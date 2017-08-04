CREATE TABLE locations (
	id UUID PRIMARY KEY,
	address TEXT, 
	rating DOUBLE PRECISION,
	rate_count INT
);

CREATE TABLE events (
	id UUID PRIMARY KEY, 
	name TEXT,
	description TEXT,
	category INT,
	latitude DOUBLE PRECISION,
	longitude DOUBLE PRECISION, 
	start_time TIMESTAMP,
	end_time TIMESTAMP,
	rating DOUBLE PRECISION,
	rate_count INT,
	price DOUBLE PRECISION, 
	capacity INT,
	reserved INT,
	custom JSONB,
	occurrence TEXT, 
	repeat_every INT, 
	repeat_on INT, 
	repeat_count INT,
	location_id UUID NOT NULL,
	FOREIGN KEY (location_id) REFERENCES locations (id)
);

CREATE TABLE images (
    id UUID PRIMARY KEY, 
    content BYTEA, 
    event_id UUID NOT NULL,
    FOREIGN KEY (event_id) REFERENCES events (id)
    );

CREATE TABLE logs(
    app_name TEXT,
    thread TEXT,
    level TEXT,
    location TEXT,
    message TEXT,
    exception TEXT,
    log_date TIMESTAMP WITH TIME ZONE
);

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "cube"; 
CREATE EXTENSION IF NOT EXISTS "earthdistance";
CREATE EXTENSION IF NOT EXISTS  pg_trgm;

CREATE INDEX idx_lat_lng ON events USING gist(ll_to_earth(latitude, longitude));


CREATE OR REPLACE FUNCTION public.check_recurrence(
    user_start timestamp without time zone,
    user_end timestamp without time zone,
    event_start timestamp without time zone,
    event_end timestamp without time zone,
    occurrence text,
    repeat_every integer,
    repeat_on integer,
    repeat_count integer)
  RETURNS boolean AS
$BODY$
DECLARE
    next_start TIMESTAMP := event_start;
    next_end TIMESTAMP := event_end;
    next_start_w TIMESTAMP := event_start;
    final_start TIMESTAMP := event_start;
    final_end TIMESTAMP := event_end;
    difference INTERVAL := event_end - event_start;
    duration INTERVAL;
    count INT := repeat_count;
    repeat_on_array INT[7] := '{}';
    array_length INT := 0;
    i INT := 1;
    j INT := 0;
    first_i INT :=0;
    weekly_add INT;
    repeats INT;
    check_repeats BOOLEAN;
    every INT := repeat_every;
    check_zero INT;
    current_month TIMESTAMP;
    days_difference INT;
BEGIN

	IF (occurrence ='none' OR occurrence = null) THEN
		RETURN ((user_start, user_end) OVERLAPS (next_start, next_end));

	ELSIF (occurrence = 'daily') THEN
		duration := interval '1 day';
		IF (repeat_count <> 0) THEN
			final_end := final_end + duration*repeat_every*repeat_count;
		ELSE
			final_end := 'infinity';
			count := 2147483646;
		END IF;
		IF (user_start > final_end) THEN 
			RETURN false;
		ELSIF (user_end < next_start) THEN 
			RETURN false;
		END IF;
		IF (user_start, user_end) OVERLAPS (next_start, next_end) THEN 
				RETURN true; 
		END IF;
		count := count - 1;
		WHILE (count > 0) LOOP
			next_start := next_start + duration*repeat_every;
			next_end := next_start + difference;
			
			IF (user_end < next_start) THEN
				RETURN false;
			END IF;

			IF (user_start, user_end) OVERLAPS (next_start, next_end) THEN 
				RETURN true; 
			END IF;
			
			count := count - 1;
		END LOOP;
		
	ELSIF (occurrence = 'weekly') THEN
		duration := interval '7 days';
		repeat_on_array := create_array (repeat_on);
		i := 7;
		WHILE (i > 0) LOOP
			EXIT WHEN(repeat_on_array[i] <> 0);
			i := i - 1;
		END LOOP;
		weekly_add := i;
		
		i := 1; 
		WHILE (i < 8) LOOP 	
			EXIT WHEN (repeat_on_array[i] <> 0);
			i := i + 1;
		END LOOP;
		first_i := i;
		i := 1;
		WHILE (i < 8) LOOP
			IF (repeat_on_array[i] <> 0) THEN
				array_length := array_length + 1;
			END IF;
			i := i + 1;
		END LOOP;
		IF (repeat_count <> 0) THEN
			IF (first_i = EXTRACT (dow FROM event_start)::int + 1 AND count = array_length) THEN
				final_end := final_end + (weekly_add - first_i)*count*( interval '1 day');
			ELSE
				final_start := final_start + duration*repeat_every - ((EXTRACT(dow FROM final_start)::int) + 1 - first_i)*(interval '1 day') + duration*repeat_every*(DIV(repeat_count, array_length) - 1);
				days_difference = repeat_count % array_length;
				WHILE (days_difference > 0) LOOP
					final_start = final_start + (interval '1 day');
					IF (repeat_on_array[EXTRACT (dow FROM final_start)::int + 1] <> 0) THEN
						days_difference := days_difference - 1;
					END IF;
				END LOOP;
				final_end := final_start + difference;
			END IF;
		ELSE 
			final_end := 'infinity'; 
			count := 2147483646;
		END IF;
		IF (user_start > final_end) THEN 
			RETURN false;
		ELSIF (user_end < next_start) THEN 
			RETURN false;
		END IF;
		i := EXTRACT (dow FROM next_start)::int + 1;
		IF (i <> first_i) THEN
			j := i;
			WHILE (j <= 7 AND count > 0) LOOP
				IF (repeat_on_array[j] <> 0) THEN
					next_start := next_start_w + (j - i)*(interval '1 day');
					next_end := next_start + difference;
					IF (user_start, user_end) OVERLAPS (next_start, next_end) THEN
						RETURN true;
					END IF;
					IF (user_end < next_start) THEN
						RETURN false;
					END IF;
					count := count - 1;
				END IF;
				j := j + 1;
			END LOOP;
			next_start_w := next_start_w + duration*repeat_every - (i - first_i)*(interval '1 day');
		END IF;
		
		WHILE (count > 0) LOOP
			IF (user_start, user_end) OVERLAPS (next_start, next_end) THEN 
				RETURN true; 
			END IF;

			---
			i := 1;
			WHILE (i <= 7 AND count > 0) LOOP 
				IF (repeat_on_array[i] <> 0) THEN
					next_start := next_start_w + (i-first_i)*(interval '1 day');
					next_end := next_start + difference;
					IF (user_start, user_end) OVERLAPS (next_start, next_end) THEN 
						RETURN true; 
					END IF;
					count := count - 1;
					IF (user_end < next_start) THEN
						RETURN false;
					END IF;
				END IF;
				i := i + 1;
			END LOOP;
			----
			next_start_w := next_start_w + duration*repeat_every;
		END LOOP;
		
		
	ELSIF (occurrence = 'monthly') THEN
		duration := interval '1 month';
		IF (repeat_count <> 0 AND EXTRACT(DAY FROM next_start)::int < 29) THEN
			final_end := final_end + (duration)*repeat_every*repeat_count;
		ELSIF (repeat_count <> 0 AND EXTRACT(DAY FROM next_start)::int < 31) THEN
			final_end := final_end + 2*(duration)*repeat_every*repeat_count;
		ELSIF (repeat_count <> 0) THEN
			final_end := final_end + 6*(duration)*repeat_every*repeat_count;
		ELSE
			final_end := 'infinity';
			count := 2147483646;
		END IF;
		
		IF (user_start > final_end) THEN 
			RETURN false;
		ELSIF (user_end < next_start) THEN 
			RETURN false;
		END IF;
		IF (user_start, user_end) OVERLAPS (next_start, next_end) THEN 
				RETURN true; 
		END IF;
		count := count - 1;
		WHILE (count > 0) LOOP
			days_difference := 1;
			repeats := 0;
			WHILE (days_difference <> 0) LOOP
				repeats := repeats + 1;
				days_difference := EXTRACT (DAY FROM (event_start)) - EXTRACT (DAY FROM(next_start + duration*repeat_every*repeats));
			END LOOP;
			next_start := next_start + repeats*repeat_every*duration;
			next_end := next_start + difference;
			
			IF (user_end < next_start) THEN
				RETURN false;
			END IF;
			
			IF (user_start, user_end) OVERLAPS (next_start, next_end) THEN 
				RETURN true; 
			END IF;
			count := count - 1;
		END LOOP;


	ELSIF (occurrence = 'monthlydayname') THEN
		duration := interval '1 month';
		IF (repeat_count <> 0) THEN
			final_end := final_end + duration*repeat_every*repeat_count;
		ELSE
			final_end := 'infinity';
			count := 2147483646;
		END IF;
		
		IF (user_start > final_end) THEN 
			RETURN false;
		ELSIF (user_end < next_start) THEN 
			RETURN false;
		END IF;
		IF (user_start, user_end) OVERLAPS (next_start, next_end) THEN 
			RETURN true; 
		END IF;
		count := count - 1;
		IF (EXTRACT (DAY FROM next_start)::int <= 28) THEN
			WHILE (count > 0) LOOP
	----------------------------------------------------
				every := repeat_every;
				WHILE (every > 0) LOOP 
					check_repeats := ((EXTRACT (DAY FROM next_start)::int -1)%7 + 29) > get_days(next_start);
					IF (check_repeats = true) THEN
						repeats := 4; 
					ELSE 
						repeats := 5;
					END IF;

					next_start := next_start + repeats*(interval '1 week');
					next_end := next_start + difference;
					every := every - 1;
				END LOOP;
	----------------------------------------------------			
				IF (user_end < next_start) THEN
					RETURN false;
				END IF;
				IF (user_start, user_end) OVERLAPS (next_start, next_end) THEN 
					RETURN true; 
				END IF;
				count := count - 1;
			END LOOP;
		ELSE
			WHILE (count > 0) LOOP
	----------------------------------------------------
				every := repeat_every;
				WHILE (every > 0) LOOP 
					current_month := next_start;
					check_repeats := ((EXTRACT (DAY FROM next_start)::int -1)%7 + 29) > get_days(next_start);
					IF (check_repeats = true) THEN
						repeats := 4; 
					ELSE 
						repeats := 5;
					END IF;

					next_start := next_start + repeats*(interval '1 week');
					next_end := next_start + difference;
					IF (EXTRACT (MONTH FROM (next_start)) <> EXTRACT (MONTH FROM(current_month + interval '1 month'))) THEN
							next_start := next_start - (interval '1 week');
							next_end := next_start + difference;
					END IF;
					every := every - 1;
				END LOOP;
	----------------------------------------------------			
				IF (user_end < next_start) THEN
					RETURN false;
				END IF;
				IF (user_start, user_end) OVERLAPS (next_start, next_end) THEN 
					RETURN true; 
				END IF;
				count := count - 1;
			END LOOP;
		END IF;
	ELSIF (occurrence = 'yearly') THEN
		IF (EXTRACT (DAY FROM next_start) = 29 AND EXTRACT (MONTH FROM next_start) = 2) THEN
				duration := (interval '1 year')*4;
		ELSE
				duration := interval '1 year';
		END IF;
		IF (repeat_count <> 0) THEN
			final_end := final_end + duration*repeat_every*repeat_count;
		ELSE
			final_end := 'infinity';
			count := 2147483646;
		END IF;
		IF (user_start > final_end) THEN 
			RETURN false;
		ELSIF (user_end < next_start) THEN 
			RETURN false;
		END IF;
		IF (user_start, user_end) OVERLAPS (next_start, next_end) THEN 
				RETURN true; 
		END IF;
		count := count - 1;
		WHILE (count > 0) LOOP			
			next_start := next_start + duration*repeat_every;
			next_end := next_start + difference;
			
			IF (user_end < next_start) THEN
				RETURN false;
			END IF;
			
			IF (user_start, user_end) OVERLAPS (next_start, next_end) THEN 
				RETURN true; 
			END IF;
			count := count - 1;
		END LOOP;
		
	END IF;
	return false;	
END;
$BODY$
  LANGUAGE plpgsql IMMUTABLE
  COST 100;
ALTER FUNCTION public.check_recurrence(timestamp without time zone, timestamp without time zone, timestamp without time zone, timestamp without time zone, text, integer, integer, integer)
  OWNER TO postgres;



CREATE OR REPLACE FUNCTION public.add_to_week(repeat_on integer)
  RETURNS integer AS
$BODY$
DECLARE
    last_day INT := 0;
    first_day INT;
    repeat_on_var INT := repeat_on;
    
BEGIN 
	IF (repeat_on >= 1) THEN
		last_day := last_day + 1; 
	END IF;
	IF (repeat_on >= 2) THEN
		last_day := last_day + 1; 
	END IF;
	IF(repeat_on >= 4) THEN
		last_day := last_day + 1; 
	END IF;
	IF (repeat_on >= 8) THEN
		last_day := last_day + 1; 
	END IF;
	IF (repeat_on >= 16) THEN
		last_day := last_day + 1; 
	END IF;
	IF (repeat_on >= 32) THEN
		last_day := last_day + 1; 
	END IF;
	IF (repeat_on >= 64) THEN
		last_day := last_day + 1; 
	END IF;

	first_day := last_day;

	WHILE (repeat_on_var > 0) LOOP
		IF ((2^(first_day-1)) <= repeat_on_var) THEN
			repeat_on_var := repeat_on_var - (2^(first_day-1));
		END IF;
		first_day := first_day - 1;
	END LOOP;

	
	
	RETURN last_day - (first_day + 1) ;	
END;
$BODY$
  LANGUAGE plpgsql IMMUTABLE
  COST 100;
ALTER FUNCTION public.add_to_week(integer)
  OWNER TO postgres;



CREATE OR REPLACE FUNCTION public.create_array(repeat_on integer)
  RETURNS integer[] AS
$BODY$
DECLARE
    ar INT[7] := '{}';
    i INT := 1;
    cat INT := repeat_on;
    mod INT;
    mult INT := 1;
BEGIN 
	WHILE (i <= 7) LOOP 
		mod := cat % 2;
		IF (mod = 1) THEN
			ar[i] := mult; 
		ELSE 
			ar[i] := 0;
		END IF;
		i := i + 1;
		mult := mult*2;
		cat := cat >> 1;
		END LOOP;
	RETURN ar;
END;
$BODY$
  LANGUAGE plpgsql IMMUTABLE
  COST 100;
ALTER FUNCTION public.create_array(integer)
  OWNER TO postgres;




