-- THIS IS THE DATABASE SETUP FOR THE AFCS-DEMO-DASHBOARD PROJECT

CREATE DATABASE afcs_demo;

--1. Stations Table
CREATE TABLE stations(
	id SERIAL PRIMARY KEY,
	name VARCHAR(100) NOT NULL,
	location VARCHAR(200),
	is_active BOOLEAN DEFAULT TRUE
);

--2. Gates table
CREATE TABLE gates(
	id SERIAL PRIMARY KEY,
	station_id INT NOT NULL REFERENCES stations(id),
	gate_number VARCHAR(10) NOT NULL,
	status VARCHAR(10) NOT NULL DEFAULT 'open'
	-- status values: open / closed / fault
);

--3. Transactions Table
CREATE TABLE transactions(
	id SERIAL PRIMARY KEY,
	gate_id INT NOT NULL REFERENCES gates(id),
	card_number VARCHAR(20) NOT NULL,
	fare_amount DECIMAL(8, 2) NOT NULL,
	transaction_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    	payment_type VARCHAR(10) NOT NULL
    -- payment_type values: card / cash / qr
);


-- SEEDing INTIAL DATA

INSERT INTO stations (name, location) VALUES
('Kalupur',       'Old City, Ahmedabad'),
('Naroda',        'Naroda, Ahmedabad'),
('RTO Circle',    'Navarangpura, Ahmedabad'),
('Vastral',       'Vastral, Ahmedabad'),
('Gheekanta',     'Gheekanta, Ahmedabad');

INSERT INTO gates (station_id, gate_number, status) VALUES
(1, 'G-01', 'open'),
(1, 'G-02', 'open'),
(2, 'G-01', 'open'),
(2, 'G-02', 'fault'),
(3, 'G-01', 'open'),
(3, 'G-02', 'closed'),
(4, 'G-01', 'open'),
(4, 'G-02', 'open'),
(5, 'G-01', 'open'),
(5, 'G-02', 'fault');

SELECT * FROM stations;
SELECT * FROM gates;
-- transactions will be empty for now — that is correct
SELECT * FROM transactions;


-- Get All Gates with Station Name
CREATE OR REPLACE FUNCTION get_all_gates_with_station_name ()
RETURNS TABLE (
	id INTEGER, 
	stationid INTEGER,
	stationname VARCHAR,
	gatenumber VARCHAR,
	status VARCHAR
)
LANGUAGE plpgsql
AS $$
BEGIN 
	RETURN QUERY
	SELECT 
		g.id, 
		s.id AS station_id, 
		s.name AS station_name, 
		g.gate_number, 
		g.status
	FROM gates g
	INNER JOIN stations s
	ON g.station_id = s.id
	ORDER BY g.id;
END
$$;

-- Get Gate by Id with stattion name
CREATE OR REPLACE FUNCTION get_gate_by_id_with_station_name (p_id INTEGER)
RETURNS TABLE (
	id INTEGER, 
	stationid INTEGER,
	stationname VARCHAR,
	gatenumber VARCHAR,
	status VARCHAR
)
LANGUAGE plpgsql
AS $$
BEGIN 
	RETURN QUERY
	SELECT 
		g.id, 
		s.id AS station_id, 
		s.name AS station_name, 
		g.gate_number, 
		g.status
	FROM gates g
	INNER JOIN stations s
	ON g.station_id = s.id
	WHERE g.id = p_id;
END
$$;

SELECT * FROM get_all_gates_with_station_name();

-- Get all transactions with gate_number

CREATE OR REPLACE FUNCTION get_all_transactions_with_gatenumber_and_stationame(recent INTEGER)
RETURNS TABLE (
	id INTEGER,
	gate_id INTEGER,
	gate_number VARCHAR,
	station_name VARCHAR,
	card_number VARCHAR,
	fare_amount NUMERIC,
	transaction_time TIMESTAMP,
	payment_type VARCHAR
)
LANGUAGE plpgsql
AS $$
BEGIN 
	RETURN QUERY
	SELECT
		t.id AS id,
		t.gate_id AS gate_id,
		g.gate_number AS gate_number,
		s.name AS station_name,
		t.card_number AS card_number,
		t.fare_amount AS fare_amount,
		t.transaction_time  AS transaction_time,
		t.payment_type AS payment_type
	FROM transactions t
	INNER JOIN gates g
		ON t.gate_id = g.id
	INNER JOIN stations s
		ON g.station_id = s.id
	ORDER BY t.transaction_time DESC
	LIMIT recent;
END
$$;


SELECT * FROM get_all_transactions_with_gatenumber_and_stationame(20);

-- Create Transactions

SELECT * FROM transactions;

CREATE OR REPLACE FUNCTION create_transaction(
	p_gate_id INTEGER, 
	p_card_number VARCHAR, 
	p_fare_amount NUMERIC, 
	p_payment_type VARCHAR
)
RETURNS TABLE (
	new_id INTEGER,
	new_transaction_time TIMESTAMP,
	station_name VARCHAR,
	gate_name VARCHAR
)
LANGUAGE plpgsql
AS $$
BEGIN
	RETURN QUERY
	WITH inserted_rows AS (
		INSERT INTO transactions (gate_id, card_number, fare_amount, payment_type)
		VALUES (p_gate_id, p_card_number, p_fare_amount, p_payment_type)
	     RETURNING transactions.id AS new_id, 
				transactions.transaction_time AS new_transaction_time, 
				transactions.gate_id	
	)
	SELECT 
	   ir.new_id AS new_id, 
        ir.new_transaction_time AS new_transaction_time, 
        s.name AS station_name, 
        g.gate_number AS gate_number
	FROM inserted_rows ir
	INNER JOIN gates g ON ir.gate_id = g.id
	INNER JOIN stations s ON g.station_id = s.id;
END
$$;

SELECT * FROM stations;
SELECT * FROM gates;

-- Queries for Stats

-- 1. Query for total_transactions, total_revenue, average_fare
SELECT 
	COUNT(*) AS total_transaction,
	COALESCE(SUM(fare_amount), 0)  AS total_revenue,
	COALESCE(AVG(fare_amount), 0) AS average_fare
FROM 
	Transactions;

--2. Query to get counts for gate status.
SELECT 
	(SELECT COUNT(id) FROM gates WHERE status = 'open') AS open_gate,
	(SELECT COUNT(id) FROM gates WHERE status = 'closed') AS close_gate,
	(SELECT COUNT(id) FROM gates WHERE status = 'fault') AS fault_gate;

--3. Hourly revenue For Today
SELECT 
	* 
FROM 
	transactions;

CREATE OR REPLACE FUNCTION get_hourly_revenue()
RETURNS TABLE(
	hr INTEGER,
	revenue NUMERIC
)
LANGUAGE plpgsql
AS $$
BEGIN
	RETURN QUERY
	SELECT 
		CAST(EXTRACT(HOUR FROM transaction_time) AS INT) AS hr,
		SUM(fare_amount) AS revenue
	FROM 
		transactions
	WHERE 
		DATE(transaction_time) = CURRENT_DATE
	GROUP BY 
		hr
	ORDER BY 
		hr;
END
$$

SELECT * FROM get_hourly_revenue();

--4. Today's Transactions per payment_type

SELECT 
	payment_type, COUNT(id) AS count
FROM 
	transactions
WHERE 
	DATE(transaction_time) = CURRENT_DATE
GROUP BY payment_type; 








