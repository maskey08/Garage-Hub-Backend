--
-- PostgreSQL database dump
--

\restrict fc9chg4mJlp09xej1o0tOOIETfX0lDxlozXuH4aREfPVKk4TbWAzn1JviW3aTJT

-- Dumped from database version 18.1
-- Dumped by pg_dump version 18.1

-- Started on 2026-04-29 09:46:54

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 242 (class 1259 OID 49547)
-- Name: appointments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.appointments (
    appointment_id integer NOT NULL,
    customer_id integer,
    vehicle_id integer,
    staff_id integer,
    scheduled_at timestamp without time zone,
    service_type character varying(50),
    status character varying(20),
    notes text
);


ALTER TABLE public.appointments OWNER TO postgres;

--
-- TOC entry 241 (class 1259 OID 49546)
-- Name: appointments_appointment_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.appointments_appointment_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.appointments_appointment_id_seq OWNER TO postgres;

--
-- TOC entry 5176 (class 0 OID 0)
-- Dependencies: 241
-- Name: appointments_appointment_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.appointments_appointment_id_seq OWNED BY public.appointments.appointment_id;


--
-- TOC entry 238 (class 1259 OID 49514)
-- Name: notifications; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.notifications (
    notification_id integer NOT NULL,
    user_id integer,
    type character varying(50),
    message text,
    is_read boolean DEFAULT false,
    sent_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.notifications OWNER TO postgres;

--
-- TOC entry 237 (class 1259 OID 49513)
-- Name: notifications_notification_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.notifications_notification_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.notifications_notification_id_seq OWNER TO postgres;

--
-- TOC entry 5177 (class 0 OID 0)
-- Dependencies: 237
-- Name: notifications_notification_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.notifications_notification_id_seq OWNED BY public.notifications.notification_id;


--
-- TOC entry 236 (class 1259 OID 49498)
-- Name: part_requests; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.part_requests (
    request_id integer NOT NULL,
    customer_id integer,
    part_name character varying(100),
    description text,
    status character varying(20),
    requested_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.part_requests OWNER TO postgres;

--
-- TOC entry 235 (class 1259 OID 49497)
-- Name: part_requests_request_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.part_requests_request_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.part_requests_request_id_seq OWNER TO postgres;

--
-- TOC entry 5178 (class 0 OID 0)
-- Dependencies: 235
-- Name: part_requests_request_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.part_requests_request_id_seq OWNED BY public.part_requests.request_id;


--
-- TOC entry 226 (class 1259 OID 49406)
-- Name: parts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.parts (
    part_id integer NOT NULL,
    vendor_id integer,
    part_name character varying(100),
    part_number character varying(50),
    category character varying(50),
    unit_price numeric(10,2),
    stock_qty integer DEFAULT 0,
    low_stock_thresh integer DEFAULT 0
);


ALTER TABLE public.parts OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 49405)
-- Name: parts_part_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.parts_part_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.parts_part_id_seq OWNER TO postgres;

--
-- TOC entry 5179 (class 0 OID 0)
-- Dependencies: 225
-- Name: parts_part_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.parts_part_id_seq OWNED BY public.parts.part_id;


--
-- TOC entry 230 (class 1259 OID 49442)
-- Name: purchase_invoice_items; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.purchase_invoice_items (
    item_id integer NOT NULL,
    purchase_id integer,
    part_id integer,
    quantity integer,
    unit_cost numeric(10,2)
);


ALTER TABLE public.purchase_invoice_items OWNER TO postgres;

--
-- TOC entry 229 (class 1259 OID 49441)
-- Name: purchase_invoice_items_item_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.purchase_invoice_items_item_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.purchase_invoice_items_item_id_seq OWNER TO postgres;

--
-- TOC entry 5180 (class 0 OID 0)
-- Dependencies: 229
-- Name: purchase_invoice_items_item_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.purchase_invoice_items_item_id_seq OWNED BY public.purchase_invoice_items.item_id;


--
-- TOC entry 228 (class 1259 OID 49423)
-- Name: purchase_invoices; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.purchase_invoices (
    purchase_id integer NOT NULL,
    vendor_id integer,
    staff_id integer,
    purchase_date date DEFAULT CURRENT_DATE,
    total_amount numeric(12,2),
    status character varying(20)
);


ALTER TABLE public.purchase_invoices OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 49422)
-- Name: purchase_invoices_purchase_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.purchase_invoices_purchase_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.purchase_invoices_purchase_id_seq OWNER TO postgres;

--
-- TOC entry 5181 (class 0 OID 0)
-- Dependencies: 227
-- Name: purchase_invoices_purchase_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.purchase_invoices_purchase_id_seq OWNED BY public.purchase_invoices.purchase_id;


--
-- TOC entry 240 (class 1259 OID 49531)
-- Name: reports; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.reports (
    report_id integer NOT NULL,
    generated_by integer,
    report_type character varying(50),
    period character varying(50),
    generated_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    file_path text
);


ALTER TABLE public.reports OWNER TO postgres;

--
-- TOC entry 239 (class 1259 OID 49530)
-- Name: reports_report_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.reports_report_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.reports_report_id_seq OWNER TO postgres;

--
-- TOC entry 5182 (class 0 OID 0)
-- Dependencies: 239
-- Name: reports_report_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.reports_report_id_seq OWNED BY public.reports.report_id;


--
-- TOC entry 244 (class 1259 OID 49572)
-- Name: reviews; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.reviews (
    review_id integer NOT NULL,
    customer_id integer,
    appointment_id integer,
    rating integer,
    comment text,
    reviewed_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT reviews_rating_check CHECK (((rating >= 1) AND (rating <= 5)))
);


ALTER TABLE public.reviews OWNER TO postgres;

--
-- TOC entry 243 (class 1259 OID 49571)
-- Name: reviews_review_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.reviews_review_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.reviews_review_id_seq OWNER TO postgres;

--
-- TOC entry 5183 (class 0 OID 0)
-- Dependencies: 243
-- Name: reviews_review_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.reviews_review_id_seq OWNED BY public.reviews.review_id;


--
-- TOC entry 234 (class 1259 OID 49480)
-- Name: sales_invoice_items; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sales_invoice_items (
    item_id integer NOT NULL,
    sale_id integer,
    part_id integer,
    quantity integer,
    unit_price numeric(10,2)
);


ALTER TABLE public.sales_invoice_items OWNER TO postgres;

--
-- TOC entry 233 (class 1259 OID 49479)
-- Name: sales_invoice_items_item_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.sales_invoice_items_item_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.sales_invoice_items_item_id_seq OWNER TO postgres;

--
-- TOC entry 5184 (class 0 OID 0)
-- Dependencies: 233
-- Name: sales_invoice_items_item_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.sales_invoice_items_item_id_seq OWNED BY public.sales_invoice_items.item_id;


--
-- TOC entry 232 (class 1259 OID 49460)
-- Name: sales_invoices; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sales_invoices (
    sale_id integer NOT NULL,
    customer_id integer,
    staff_id integer,
    sale_date date DEFAULT CURRENT_DATE,
    subtotal numeric(12,2),
    discount_applied numeric(12,2),
    total_amount numeric(12,2),
    payment_status character varying(20),
    credit_used boolean DEFAULT false
);


ALTER TABLE public.sales_invoices OWNER TO postgres;

--
-- TOC entry 231 (class 1259 OID 49459)
-- Name: sales_invoices_sale_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.sales_invoices_sale_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.sales_invoices_sale_id_seq OWNER TO postgres;

--
-- TOC entry 5185 (class 0 OID 0)
-- Dependencies: 231
-- Name: sales_invoices_sale_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.sales_invoices_sale_id_seq OWNED BY public.sales_invoices.sale_id;


--
-- TOC entry 220 (class 1259 OID 49356)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    user_id integer NOT NULL,
    first_name character varying(50),
    last_name character varying(50),
    email character varying(100),
    phone character varying(20),
    password_hash text NOT NULL,
    role character varying(20),
    total_spent numeric(12,2) DEFAULT 0,
    credit_balance numeric(12,2) DEFAULT 0,
    credit_due_date date,
    managed_by integer,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 49355)
-- Name: users_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.users_user_id_seq OWNER TO postgres;

--
-- TOC entry 5186 (class 0 OID 0)
-- Dependencies: 219
-- Name: users_user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_user_id_seq OWNED BY public.users.user_id;


--
-- TOC entry 222 (class 1259 OID 49377)
-- Name: vehicles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.vehicles (
    vehicle_id integer NOT NULL,
    user_id integer,
    vehicle_number character varying(20),
    make character varying(50),
    model character varying(50),
    year integer,
    vin character varying(50)
);


ALTER TABLE public.vehicles OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 49376)
-- Name: vehicles_vehicle_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.vehicles_vehicle_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.vehicles_vehicle_id_seq OWNER TO postgres;

--
-- TOC entry 5187 (class 0 OID 0)
-- Dependencies: 221
-- Name: vehicles_vehicle_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.vehicles_vehicle_id_seq OWNED BY public.vehicles.vehicle_id;


--
-- TOC entry 224 (class 1259 OID 49394)
-- Name: vendors; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.vendors (
    vendor_id integer NOT NULL,
    name character varying(100),
    contact_person character varying(100),
    email character varying(100),
    phone character varying(20),
    address text
);


ALTER TABLE public.vendors OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 49393)
-- Name: vendors_vendor_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.vendors_vendor_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.vendors_vendor_id_seq OWNER TO postgres;

--
-- TOC entry 5188 (class 0 OID 0)
-- Dependencies: 223
-- Name: vendors_vendor_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.vendors_vendor_id_seq OWNED BY public.vendors.vendor_id;


--
-- TOC entry 4939 (class 2604 OID 49550)
-- Name: appointments appointment_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.appointments ALTER COLUMN appointment_id SET DEFAULT nextval('public.appointments_appointment_id_seq'::regclass);


--
-- TOC entry 4934 (class 2604 OID 49517)
-- Name: notifications notification_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notifications ALTER COLUMN notification_id SET DEFAULT nextval('public.notifications_notification_id_seq'::regclass);


--
-- TOC entry 4932 (class 2604 OID 49501)
-- Name: part_requests request_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.part_requests ALTER COLUMN request_id SET DEFAULT nextval('public.part_requests_request_id_seq'::regclass);


--
-- TOC entry 4922 (class 2604 OID 49409)
-- Name: parts part_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.parts ALTER COLUMN part_id SET DEFAULT nextval('public.parts_part_id_seq'::regclass);


--
-- TOC entry 4927 (class 2604 OID 49445)
-- Name: purchase_invoice_items item_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.purchase_invoice_items ALTER COLUMN item_id SET DEFAULT nextval('public.purchase_invoice_items_item_id_seq'::regclass);


--
-- TOC entry 4925 (class 2604 OID 49426)
-- Name: purchase_invoices purchase_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.purchase_invoices ALTER COLUMN purchase_id SET DEFAULT nextval('public.purchase_invoices_purchase_id_seq'::regclass);


--
-- TOC entry 4937 (class 2604 OID 49534)
-- Name: reports report_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reports ALTER COLUMN report_id SET DEFAULT nextval('public.reports_report_id_seq'::regclass);


--
-- TOC entry 4940 (class 2604 OID 49575)
-- Name: reviews review_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reviews ALTER COLUMN review_id SET DEFAULT nextval('public.reviews_review_id_seq'::regclass);


--
-- TOC entry 4931 (class 2604 OID 49483)
-- Name: sales_invoice_items item_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sales_invoice_items ALTER COLUMN item_id SET DEFAULT nextval('public.sales_invoice_items_item_id_seq'::regclass);


--
-- TOC entry 4928 (class 2604 OID 49463)
-- Name: sales_invoices sale_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sales_invoices ALTER COLUMN sale_id SET DEFAULT nextval('public.sales_invoices_sale_id_seq'::regclass);


--
-- TOC entry 4916 (class 2604 OID 49359)
-- Name: users user_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN user_id SET DEFAULT nextval('public.users_user_id_seq'::regclass);


--
-- TOC entry 4920 (class 2604 OID 49380)
-- Name: vehicles vehicle_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vehicles ALTER COLUMN vehicle_id SET DEFAULT nextval('public.vehicles_vehicle_id_seq'::regclass);


--
-- TOC entry 4921 (class 2604 OID 49397)
-- Name: vendors vendor_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vendors ALTER COLUMN vendor_id SET DEFAULT nextval('public.vendors_vendor_id_seq'::regclass);


--
-- TOC entry 5168 (class 0 OID 49547)
-- Dependencies: 242
-- Data for Name: appointments; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.appointments (appointment_id, customer_id, vehicle_id, staff_id, scheduled_at, service_type, status, notes) FROM stdin;
\.


--
-- TOC entry 5164 (class 0 OID 49514)
-- Dependencies: 238
-- Data for Name: notifications; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.notifications (notification_id, user_id, type, message, is_read, sent_at) FROM stdin;
\.


--
-- TOC entry 5162 (class 0 OID 49498)
-- Dependencies: 236
-- Data for Name: part_requests; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.part_requests (request_id, customer_id, part_name, description, status, requested_at) FROM stdin;
\.


--
-- TOC entry 5152 (class 0 OID 49406)
-- Dependencies: 226
-- Data for Name: parts; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.parts (part_id, vendor_id, part_name, part_number, category, unit_price, stock_qty, low_stock_thresh) FROM stdin;
\.


--
-- TOC entry 5156 (class 0 OID 49442)
-- Dependencies: 230
-- Data for Name: purchase_invoice_items; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.purchase_invoice_items (item_id, purchase_id, part_id, quantity, unit_cost) FROM stdin;
\.


--
-- TOC entry 5154 (class 0 OID 49423)
-- Dependencies: 228
-- Data for Name: purchase_invoices; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.purchase_invoices (purchase_id, vendor_id, staff_id, purchase_date, total_amount, status) FROM stdin;
\.


--
-- TOC entry 5166 (class 0 OID 49531)
-- Dependencies: 240
-- Data for Name: reports; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.reports (report_id, generated_by, report_type, period, generated_at, file_path) FROM stdin;
\.


--
-- TOC entry 5170 (class 0 OID 49572)
-- Dependencies: 244
-- Data for Name: reviews; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.reviews (review_id, customer_id, appointment_id, rating, comment, reviewed_at) FROM stdin;
\.


--
-- TOC entry 5160 (class 0 OID 49480)
-- Dependencies: 234
-- Data for Name: sales_invoice_items; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.sales_invoice_items (item_id, sale_id, part_id, quantity, unit_price) FROM stdin;
\.


--
-- TOC entry 5158 (class 0 OID 49460)
-- Dependencies: 232
-- Data for Name: sales_invoices; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.sales_invoices (sale_id, customer_id, staff_id, sale_date, subtotal, discount_applied, total_amount, payment_status, credit_used) FROM stdin;
\.


--
-- TOC entry 5146 (class 0 OID 49356)
-- Dependencies: 220
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (user_id, first_name, last_name, email, phone, password_hash, role, total_spent, credit_balance, credit_due_date, managed_by, created_at) FROM stdin;
\.


--
-- TOC entry 5148 (class 0 OID 49377)
-- Dependencies: 222
-- Data for Name: vehicles; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.vehicles (vehicle_id, user_id, vehicle_number, make, model, year, vin) FROM stdin;
\.


--
-- TOC entry 5150 (class 0 OID 49394)
-- Dependencies: 224
-- Data for Name: vendors; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.vendors (vendor_id, name, contact_person, email, phone, address) FROM stdin;
\.


--
-- TOC entry 5189 (class 0 OID 0)
-- Dependencies: 241
-- Name: appointments_appointment_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.appointments_appointment_id_seq', 1, false);


--
-- TOC entry 5190 (class 0 OID 0)
-- Dependencies: 237
-- Name: notifications_notification_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.notifications_notification_id_seq', 1, false);


--
-- TOC entry 5191 (class 0 OID 0)
-- Dependencies: 235
-- Name: part_requests_request_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.part_requests_request_id_seq', 1, false);


--
-- TOC entry 5192 (class 0 OID 0)
-- Dependencies: 225
-- Name: parts_part_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.parts_part_id_seq', 1, false);


--
-- TOC entry 5193 (class 0 OID 0)
-- Dependencies: 229
-- Name: purchase_invoice_items_item_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.purchase_invoice_items_item_id_seq', 1, false);


--
-- TOC entry 5194 (class 0 OID 0)
-- Dependencies: 227
-- Name: purchase_invoices_purchase_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.purchase_invoices_purchase_id_seq', 1, false);


--
-- TOC entry 5195 (class 0 OID 0)
-- Dependencies: 239
-- Name: reports_report_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.reports_report_id_seq', 1, false);


--
-- TOC entry 5196 (class 0 OID 0)
-- Dependencies: 243
-- Name: reviews_review_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.reviews_review_id_seq', 1, false);


--
-- TOC entry 5197 (class 0 OID 0)
-- Dependencies: 233
-- Name: sales_invoice_items_item_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.sales_invoice_items_item_id_seq', 1, false);


--
-- TOC entry 5198 (class 0 OID 0)
-- Dependencies: 231
-- Name: sales_invoices_sale_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.sales_invoices_sale_id_seq', 1, false);


--
-- TOC entry 5199 (class 0 OID 0)
-- Dependencies: 219
-- Name: users_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_user_id_seq', 1, false);


--
-- TOC entry 5200 (class 0 OID 0)
-- Dependencies: 221
-- Name: vehicles_vehicle_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.vehicles_vehicle_id_seq', 1, false);


--
-- TOC entry 5201 (class 0 OID 0)
-- Dependencies: 223
-- Name: vendors_vendor_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.vendors_vendor_id_seq', 1, false);


--
-- TOC entry 4976 (class 2606 OID 49555)
-- Name: appointments appointments_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.appointments
    ADD CONSTRAINT appointments_pkey PRIMARY KEY (appointment_id);


--
-- TOC entry 4972 (class 2606 OID 49524)
-- Name: notifications notifications_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notifications
    ADD CONSTRAINT notifications_pkey PRIMARY KEY (notification_id);


--
-- TOC entry 4970 (class 2606 OID 49507)
-- Name: part_requests part_requests_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.part_requests
    ADD CONSTRAINT part_requests_pkey PRIMARY KEY (request_id);


--
-- TOC entry 4958 (class 2606 OID 49416)
-- Name: parts parts_part_number_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.parts
    ADD CONSTRAINT parts_part_number_key UNIQUE (part_number);


--
-- TOC entry 4960 (class 2606 OID 49414)
-- Name: parts parts_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.parts
    ADD CONSTRAINT parts_pkey PRIMARY KEY (part_id);


--
-- TOC entry 4964 (class 2606 OID 49448)
-- Name: purchase_invoice_items purchase_invoice_items_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.purchase_invoice_items
    ADD CONSTRAINT purchase_invoice_items_pkey PRIMARY KEY (item_id);


--
-- TOC entry 4962 (class 2606 OID 49430)
-- Name: purchase_invoices purchase_invoices_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.purchase_invoices
    ADD CONSTRAINT purchase_invoices_pkey PRIMARY KEY (purchase_id);


--
-- TOC entry 4974 (class 2606 OID 49540)
-- Name: reports reports_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reports
    ADD CONSTRAINT reports_pkey PRIMARY KEY (report_id);


--
-- TOC entry 4978 (class 2606 OID 49582)
-- Name: reviews reviews_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reviews
    ADD CONSTRAINT reviews_pkey PRIMARY KEY (review_id);


--
-- TOC entry 4968 (class 2606 OID 49486)
-- Name: sales_invoice_items sales_invoice_items_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sales_invoice_items
    ADD CONSTRAINT sales_invoice_items_pkey PRIMARY KEY (item_id);


--
-- TOC entry 4966 (class 2606 OID 49468)
-- Name: sales_invoices sales_invoices_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sales_invoices
    ADD CONSTRAINT sales_invoices_pkey PRIMARY KEY (sale_id);


--
-- TOC entry 4944 (class 2606 OID 49370)
-- Name: users users_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_email_key UNIQUE (email);


--
-- TOC entry 4946 (class 2606 OID 49368)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (user_id);


--
-- TOC entry 4948 (class 2606 OID 49383)
-- Name: vehicles vehicles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vehicles
    ADD CONSTRAINT vehicles_pkey PRIMARY KEY (vehicle_id);


--
-- TOC entry 4950 (class 2606 OID 49385)
-- Name: vehicles vehicles_vehicle_number_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vehicles
    ADD CONSTRAINT vehicles_vehicle_number_key UNIQUE (vehicle_number);


--
-- TOC entry 4952 (class 2606 OID 49387)
-- Name: vehicles vehicles_vin_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vehicles
    ADD CONSTRAINT vehicles_vin_key UNIQUE (vin);


--
-- TOC entry 4954 (class 2606 OID 49404)
-- Name: vendors vendors_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vendors
    ADD CONSTRAINT vendors_email_key UNIQUE (email);


--
-- TOC entry 4956 (class 2606 OID 49402)
-- Name: vendors vendors_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vendors
    ADD CONSTRAINT vendors_pkey PRIMARY KEY (vendor_id);


--
-- TOC entry 4993 (class 2606 OID 49556)
-- Name: appointments appointments_customer_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.appointments
    ADD CONSTRAINT appointments_customer_id_fkey FOREIGN KEY (customer_id) REFERENCES public.users(user_id);


--
-- TOC entry 4994 (class 2606 OID 49566)
-- Name: appointments appointments_staff_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.appointments
    ADD CONSTRAINT appointments_staff_id_fkey FOREIGN KEY (staff_id) REFERENCES public.users(user_id);


--
-- TOC entry 4995 (class 2606 OID 49561)
-- Name: appointments appointments_vehicle_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.appointments
    ADD CONSTRAINT appointments_vehicle_id_fkey FOREIGN KEY (vehicle_id) REFERENCES public.vehicles(vehicle_id);


--
-- TOC entry 4991 (class 2606 OID 49525)
-- Name: notifications notifications_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notifications
    ADD CONSTRAINT notifications_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id);


--
-- TOC entry 4990 (class 2606 OID 49508)
-- Name: part_requests part_requests_customer_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.part_requests
    ADD CONSTRAINT part_requests_customer_id_fkey FOREIGN KEY (customer_id) REFERENCES public.users(user_id);


--
-- TOC entry 4981 (class 2606 OID 49417)
-- Name: parts parts_vendor_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.parts
    ADD CONSTRAINT parts_vendor_id_fkey FOREIGN KEY (vendor_id) REFERENCES public.vendors(vendor_id);


--
-- TOC entry 4984 (class 2606 OID 49454)
-- Name: purchase_invoice_items purchase_invoice_items_part_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.purchase_invoice_items
    ADD CONSTRAINT purchase_invoice_items_part_id_fkey FOREIGN KEY (part_id) REFERENCES public.parts(part_id);


--
-- TOC entry 4985 (class 2606 OID 49449)
-- Name: purchase_invoice_items purchase_invoice_items_purchase_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.purchase_invoice_items
    ADD CONSTRAINT purchase_invoice_items_purchase_id_fkey FOREIGN KEY (purchase_id) REFERENCES public.purchase_invoices(purchase_id) ON DELETE CASCADE;


--
-- TOC entry 4982 (class 2606 OID 49436)
-- Name: purchase_invoices purchase_invoices_staff_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.purchase_invoices
    ADD CONSTRAINT purchase_invoices_staff_id_fkey FOREIGN KEY (staff_id) REFERENCES public.users(user_id);


--
-- TOC entry 4983 (class 2606 OID 49431)
-- Name: purchase_invoices purchase_invoices_vendor_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.purchase_invoices
    ADD CONSTRAINT purchase_invoices_vendor_id_fkey FOREIGN KEY (vendor_id) REFERENCES public.vendors(vendor_id);


--
-- TOC entry 4992 (class 2606 OID 49541)
-- Name: reports reports_generated_by_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reports
    ADD CONSTRAINT reports_generated_by_fkey FOREIGN KEY (generated_by) REFERENCES public.users(user_id);


--
-- TOC entry 4996 (class 2606 OID 49588)
-- Name: reviews reviews_appointment_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reviews
    ADD CONSTRAINT reviews_appointment_id_fkey FOREIGN KEY (appointment_id) REFERENCES public.appointments(appointment_id);


--
-- TOC entry 4997 (class 2606 OID 49583)
-- Name: reviews reviews_customer_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reviews
    ADD CONSTRAINT reviews_customer_id_fkey FOREIGN KEY (customer_id) REFERENCES public.users(user_id);


--
-- TOC entry 4988 (class 2606 OID 49492)
-- Name: sales_invoice_items sales_invoice_items_part_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sales_invoice_items
    ADD CONSTRAINT sales_invoice_items_part_id_fkey FOREIGN KEY (part_id) REFERENCES public.parts(part_id);


--
-- TOC entry 4989 (class 2606 OID 49487)
-- Name: sales_invoice_items sales_invoice_items_sale_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sales_invoice_items
    ADD CONSTRAINT sales_invoice_items_sale_id_fkey FOREIGN KEY (sale_id) REFERENCES public.sales_invoices(sale_id) ON DELETE CASCADE;


--
-- TOC entry 4986 (class 2606 OID 49469)
-- Name: sales_invoices sales_invoices_customer_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sales_invoices
    ADD CONSTRAINT sales_invoices_customer_id_fkey FOREIGN KEY (customer_id) REFERENCES public.users(user_id);


--
-- TOC entry 4987 (class 2606 OID 49474)
-- Name: sales_invoices sales_invoices_staff_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sales_invoices
    ADD CONSTRAINT sales_invoices_staff_id_fkey FOREIGN KEY (staff_id) REFERENCES public.users(user_id);


--
-- TOC entry 4979 (class 2606 OID 49371)
-- Name: users users_managed_by_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_managed_by_fkey FOREIGN KEY (managed_by) REFERENCES public.users(user_id);


--
-- TOC entry 4980 (class 2606 OID 49388)
-- Name: vehicles vehicles_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vehicles
    ADD CONSTRAINT vehicles_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(user_id);


-- Completed on 2026-04-29 09:46:54

--
-- PostgreSQL database dump complete
--

\unrestrict fc9chg4mJlp09xej1o0tOOIETfX0lDxlozXuH4aREfPVKk4TbWAzn1JviW3aTJT

