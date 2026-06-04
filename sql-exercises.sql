-- ============================================================
-- ANSI SQL Using MySQL
-- Database: Community Event Portal
-- ============================================================


-- ── CREATE DATABASE ──────────────────────────────────────────
CREATE DATABASE IF NOT EXISTS community_portal;
USE community_portal;


-- ── TABLE: Users ─────────────────────────────────────────────
CREATE TABLE Users (
    user_id           INT          PRIMARY KEY AUTO_INCREMENT,
    full_name         VARCHAR(100) NOT NULL,
    email             VARCHAR(100) NOT NULL UNIQUE,
    city              VARCHAR(100) NOT NULL,
    registration_date DATE         NOT NULL
);


-- ── TABLE: Events ────────────────────────────────────────────
CREATE TABLE Events (
    event_id     INT          PRIMARY KEY AUTO_INCREMENT,
    title        VARCHAR(200) NOT NULL,
    description  TEXT,
    city         VARCHAR(100) NOT NULL,
    start_date   DATETIME     NOT NULL,
    end_date     DATETIME     NOT NULL,
    status       ENUM('upcoming','completed','cancelled') NOT NULL,
    organizer_id INT,
    FOREIGN KEY (organizer_id) REFERENCES Users(user_id)
);


-- ── TABLE: Sessions ──────────────────────────────────────────
CREATE TABLE Sessions (
    session_id   INT          PRIMARY KEY AUTO_INCREMENT,
    event_id     INT          NOT NULL,
    title        VARCHAR(200) NOT NULL,
    speaker_name VARCHAR(100) NOT NULL,
    start_time   DATETIME     NOT NULL,
    end_time     DATETIME     NOT NULL,
    FOREIGN KEY (event_id) REFERENCES Events(event_id)
);


-- ── TABLE: Registrations ─────────────────────────────────────
CREATE TABLE Registrations (
    registration_id   INT  PRIMARY KEY AUTO_INCREMENT,
    user_id           INT  NOT NULL,
    event_id          INT  NOT NULL,
    registration_date DATE NOT NULL,
    FOREIGN KEY (user_id)  REFERENCES Users(user_id),
    FOREIGN KEY (event_id) REFERENCES Events(event_id)
);


-- ── TABLE: Feedback ──────────────────────────────────────────
CREATE TABLE Feedback (
    feedback_id   INT  PRIMARY KEY AUTO_INCREMENT,
    user_id       INT  NOT NULL,
    event_id      INT  NOT NULL,
    rating        INT  CHECK (rating BETWEEN 1 AND 5),
    comments      TEXT,
    feedback_date DATE NOT NULL,
    FOREIGN KEY (user_id)  REFERENCES Users(user_id),
    FOREIGN KEY (event_id) REFERENCES Events(event_id)
);


-- ── TABLE: Resources ─────────────────────────────────────────
CREATE TABLE Resources (
    resource_id   INT          PRIMARY KEY AUTO_INCREMENT,
    event_id      INT          NOT NULL,
    resource_type ENUM('pdf','image','link') NOT NULL,
    resource_url  VARCHAR(255) NOT NULL,
    uploaded_at   DATETIME     NOT NULL,
    FOREIGN KEY (event_id) REFERENCES Events(event_id)
);


-- ============================================================
-- SAMPLE DATA
-- ============================================================

INSERT INTO Users (full_name, email, city, registration_date) VALUES
('Alice Johnson', 'alice@example.com', 'New York',    '2024-12-01'),
('Bob Smith',     'bob@example.com',   'Los Angeles', '2024-12-05'),
('Charlie Lee',   'charlie@example.com','Chicago',    '2024-12-10'),
('Diana King',    'diana@example.com', 'New York',    '2025-01-15'),
('Ethan Hunt',    'ethan@example.com', 'Los Angeles', '2025-02-01');

INSERT INTO Events (title, description, city, start_date, end_date, status, organizer_id) VALUES
('Tech Innovators Meetup',       'A meetup for tech enthusiasts.',        'New York',    '2025-06-10 10:00:00', '2025-06-10 16:00:00', 'upcoming',  1),
('AI & ML Conference',           'Conference on AI and ML advancements.', 'Chicago',     '2025-05-15 09:00:00', '2025-05-15 17:00:00', 'completed', 3),
('Frontend Development Bootcamp','Hands-on training on frontend tech.',   'Los Angeles', '2025-07-01 10:00:00', '2025-07-03 16:00:00', 'upcoming',  2);

INSERT INTO Sessions (event_id, title, speaker_name, start_time, end_time) VALUES
(1, 'Opening Keynote',   'Dr. Tech',      '2025-06-10 10:00:00', '2025-06-10 11:00:00'),
(1, 'Future of Web Dev', 'Alice Johnson', '2025-06-10 11:15:00', '2025-06-10 12:30:00'),
(2, 'AI in Healthcare',  'Charlie Lee',   '2025-05-15 09:30:00', '2025-05-15 11:00:00'),
(3, 'Intro to HTML5',    'Bob Smith',     '2025-07-01 10:00:00', '2025-07-01 12:00:00');

INSERT INTO Registrations (user_id, event_id, registration_date) VALUES
(1, 1, '2025-05-01'),
(2, 1, '2025-05-02'),
(3, 2, '2025-04-30'),
(4, 2, '2025-04-28'),
(5, 3, '2025-06-15');

INSERT INTO Feedback (user_id, event_id, rating, comments, feedback_date) VALUES
(3, 2, 4, 'Great insights!',    '2025-05-16'),
(4, 2, 5, 'Very informative.',  '2025-05-16'),
(2, 1, 3, 'Could be better.',   '2025-06-11');

INSERT INTO Resources (event_id, resource_type, resource_url, uploaded_at) VALUES
(1, 'pdf',   'https://portal.com/resources/tech_meetup_agenda.pdf', '2025-05-01 10:00:00'),
(2, 'image', 'https://portal.com/resources/ai_poster.jpg',          '2025-04-20 09:00:00'),
(3, 'link',  'https://portal.com/resources/html5_docs',             '2025-06-25 15:00:00');


-- ============================================================
-- EXERCISES
-- ============================================================


-- ── Exercise 1: User Upcoming Events ─────────────────────────
-- Show all upcoming events a user is registered for in their city, sorted by date.

SELECT
    u.full_name,
    e.title,
    e.city,
    e.start_date
FROM Registrations r
JOIN Users  u ON r.user_id  = u.user_id
JOIN Events e ON r.event_id = e.event_id
WHERE e.status = 'upcoming'
  AND e.city   = u.city
ORDER BY e.start_date;


-- ── Exercise 2: Top Rated Events ─────────────────────────────
-- Events with the highest average rating (min 10 feedback submissions).

SELECT
    e.title,
    ROUND(AVG(f.rating), 2) AS avg_rating,
    COUNT(f.feedback_id)    AS total_feedback
FROM Feedback f
JOIN Events e ON f.event_id = e.event_id
GROUP BY e.event_id, e.title
HAVING COUNT(f.feedback_id) >= 10
ORDER BY avg_rating DESC;


-- ── Exercise 3: Inactive Users ────────────────────────────────
-- Users who have not registered for any event in the last 90 days.

SELECT u.user_id, u.full_name, u.email
FROM Users u
WHERE u.user_id NOT IN (
    SELECT DISTINCT user_id
    FROM Registrations
    WHERE registration_date >= CURDATE() - INTERVAL 90 DAY
);


-- ── Exercise 4: Peak Session Hours ───────────────────────────
-- Sessions scheduled between 10 AM and 12 PM per event.

SELECT
    e.title,
    COUNT(s.session_id) AS sessions_10_to_12
FROM Sessions s
JOIN Events e ON s.event_id = e.event_id
WHERE TIME(s.start_time) >= '10:00:00'
  AND TIME(s.start_time) <  '12:00:00'
GROUP BY e.event_id, e.title;


-- ── Exercise 5: Most Active Cities ───────────────────────────
-- Top 5 cities by distinct user registrations.

SELECT
    u.city,
    COUNT(DISTINCT r.user_id) AS total_registrations
FROM Registrations r
JOIN Users u ON r.user_id = u.user_id
GROUP BY u.city
ORDER BY total_registrations DESC
LIMIT 5;


-- ── Exercise 6: Event Resource Summary ───────────────────────
-- Number of PDFs, images, and links uploaded per event.

SELECT
    e.title,
    SUM(CASE WHEN r.resource_type = 'pdf'   THEN 1 ELSE 0 END) AS pdfs,
    SUM(CASE WHEN r.resource_type = 'image' THEN 1 ELSE 0 END) AS images,
    SUM(CASE WHEN r.resource_type = 'link'  THEN 1 ELSE 0 END) AS links,
    COUNT(r.resource_id) AS total_resources
FROM Events e
LEFT JOIN Resources r ON e.event_id = r.event_id
GROUP BY e.event_id, e.title;


-- ── Exercise 7: Low Feedback Alerts ──────────────────────────
-- Users who gave a rating less than 3, with comments and event name.

SELECT
    u.full_name,
    e.title      AS event_name,
    f.rating,
    f.comments
FROM Feedback f
JOIN Users  u ON f.user_id  = u.user_id
JOIN Events e ON f.event_id = e.event_id
WHERE f.rating < 3;


-- ── Exercise 8: Sessions per Upcoming Event ───────────────────
-- Upcoming events with their session count.

SELECT
    e.title,
    e.start_date,
    COUNT(s.session_id) AS session_count
FROM Events e
LEFT JOIN Sessions s ON e.event_id = s.event_id
WHERE e.status = 'upcoming'
GROUP BY e.event_id, e.title, e.start_date;


-- ── Exercise 9: Organizer Event Summary ──────────────────────
-- Per organizer: number of events per status.

SELECT
    u.full_name       AS organizer,
    e.status,
    COUNT(e.event_id) AS event_count
FROM Events e
JOIN Users u ON e.organizer_id = u.user_id
GROUP BY u.user_id, u.full_name, e.status
ORDER BY u.full_name;


-- ── Exercise 10: Feedback Gap ────────────────────────────────
-- Events that had registrations but no feedback.

SELECT
    e.event_id,
    e.title
FROM Events e
WHERE e.event_id IN (SELECT DISTINCT event_id FROM Registrations)
  AND e.event_id NOT IN (SELECT DISTINCT event_id FROM Feedback);


-- ── Exercise 11: Daily New User Count ────────────────────────
-- Users registered each day in the last 7 days.

SELECT
    registration_date,
    COUNT(user_id) AS new_users
FROM Users
WHERE registration_date >= CURDATE() - INTERVAL 7 DAY
GROUP BY registration_date
ORDER BY registration_date;


-- ── Exercise 12: Event with Maximum Sessions ──────────────────
-- Event(s) with the highest number of sessions.

SELECT
    e.title,
    COUNT(s.session_id) AS session_count
FROM Sessions s
JOIN Events e ON s.event_id = e.event_id
GROUP BY e.event_id, e.title
HAVING COUNT(s.session_id) = (
    SELECT MAX(cnt)
    FROM (
        SELECT COUNT(session_id) AS cnt
        FROM Sessions
        GROUP BY event_id
    ) AS counts
);


-- ── Exercise 13: Average Rating per City ─────────────────────
-- Average feedback rating for events in each city.

SELECT
    e.city,
    ROUND(AVG(f.rating), 2) AS avg_rating
FROM Feedback f
JOIN Events e ON f.event_id = e.event_id
GROUP BY e.city
ORDER BY avg_rating DESC;


-- ── Exercise 14: Most Registered Events ──────────────────────
-- Top 3 events by total registrations.

SELECT
    e.title,
    COUNT(r.registration_id) AS total_registrations
FROM Registrations r
JOIN Events e ON r.event_id = e.event_id
GROUP BY e.event_id, e.title
ORDER BY total_registrations DESC
LIMIT 3;


-- ── Exercise 15: Event Session Time Conflict ──────────────────
-- Overlapping sessions within the same event.

SELECT
    a.event_id,
    a.title        AS session_a,
    b.title        AS session_b,
    a.start_time   AS a_start,
    a.end_time     AS a_end,
    b.start_time   AS b_start,
    b.end_time     AS b_end
FROM Sessions a
JOIN Sessions b
  ON  a.event_id   = b.event_id
  AND a.session_id < b.session_id
  AND a.start_time < b.end_time
  AND a.end_time   > b.start_time;


-- ── Exercise 16: Unregistered Active Users ────────────────────
-- Users who signed up in the last 30 days but haven't registered for any event.

SELECT u.user_id, u.full_name, u.email, u.registration_date
FROM Users u
WHERE u.registration_date >= CURDATE() - INTERVAL 30 DAY
  AND u.user_id NOT IN (SELECT DISTINCT user_id FROM Registrations);


-- ── Exercise 17: Multi-Session Speakers ──────────────────────
-- Speakers handling more than one session across all events.

SELECT
    speaker_name,
    COUNT(session_id) AS session_count
FROM Sessions
GROUP BY speaker_name
HAVING COUNT(session_id) > 1;


-- ── Exercise 18: Resource Availability Check ─────────────────
-- Events that have no resources uploaded.

SELECT e.event_id, e.title
FROM Events e
WHERE e.event_id NOT IN (SELECT DISTINCT event_id FROM Resources);


-- ── Exercise 19: Completed Events with Feedback Summary ───────
-- Total registrations and average rating for completed events.

SELECT
    e.title,
    COUNT(DISTINCT r.registration_id) AS total_registrations,
    ROUND(AVG(f.rating), 2)           AS avg_rating
FROM Events e
LEFT JOIN Registrations r ON e.event_id = r.event_id
LEFT JOIN Feedback       f ON e.event_id = f.event_id
WHERE e.status = 'completed'
GROUP BY e.event_id, e.title;


-- ── Exercise 20: User Engagement Index ───────────────────────
-- Per user: events attended and feedbacks submitted.

SELECT
    u.full_name,
    COUNT(DISTINCT r.event_id)   AS events_attended,
    COUNT(DISTINCT f.feedback_id) AS feedbacks_submitted
FROM Users u
LEFT JOIN Registrations r ON u.user_id = r.user_id
LEFT JOIN Feedback       f ON u.user_id = f.user_id
GROUP BY u.user_id, u.full_name
ORDER BY events_attended DESC;


-- ── Exercise 21: Top Feedback Providers ──────────────────────
-- Top 5 users with the most feedback entries.

SELECT
    u.full_name,
    COUNT(f.feedback_id) AS feedback_count
FROM Feedback f
JOIN Users u ON f.user_id = u.user_id
GROUP BY u.user_id, u.full_name
ORDER BY feedback_count DESC
LIMIT 5;


-- ── Exercise 22: Duplicate Registrations Check ────────────────
-- Users registered more than once for the same event.

SELECT
    user_id,
    event_id,
    COUNT(registration_id) AS reg_count
FROM Registrations
GROUP BY user_id, event_id
HAVING COUNT(registration_id) > 1;


-- ── Exercise 23: Registration Trends ─────────────────────────
-- Month-wise registration count for the past 12 months.

SELECT
    DATE_FORMAT(registration_date, '%Y-%m') AS month,
    COUNT(registration_id)                  AS registrations
FROM Registrations
WHERE registration_date >= CURDATE() - INTERVAL 12 MONTH
GROUP BY DATE_FORMAT(registration_date, '%Y-%m')
ORDER BY month;


-- ── Exercise 24: Average Session Duration per Event ───────────
-- Average session duration in minutes per event.

SELECT
    e.title,
    ROUND(AVG(TIMESTAMPDIFF(MINUTE, s.start_time, s.end_time)), 2) AS avg_duration_minutes
FROM Sessions s
JOIN Events e ON s.event_id = e.event_id
GROUP BY e.event_id, e.title;


-- ── Exercise 25: Events Without Sessions ─────────────────────
-- Events that have no sessions scheduled.

SELECT e.event_id, e.title, e.status
FROM Events e
WHERE e.event_id NOT IN (SELECT DISTINCT event_id FROM Sessions);
