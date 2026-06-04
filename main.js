/* ============================================================
   main.js – Local Community Event Portal
   JavaScript Exercises 1–14
   ============================================================ */


/* ── Exercise 1: Basics & Setup ── */
console.log("Welcome to the Community Portal");

window.addEventListener("load", function () {
  alert("Page fully loaded – Community Event Portal");
});


/* ── Shared event data used across exercises ── */
var events = [
  { id: 1, name: "Street Fair",      date: "2025-08-15", category: "food",     seats: 20, past: false },
  { id: 2, name: "Music Night",      date: "2025-09-05", category: "music",    seats: 0,  past: false },
  { id: 3, name: "City Marathon",    date: "2025-07-10", category: "sports",   seats: 50, past: true  },
  { id: 4, name: "Baking Workshop",  date: "2025-09-20", category: "workshop", seats: 15, past: false },
  { id: 5, name: "Jazz Evening",     date: "2025-10-01", category: "music",    seats: 30, past: false },
  { id: 6, name: "Food Festival",    date: "2025-10-12", category: "food",     seats: 100,past: false }
];


/* ── Exercise 2: Syntax, Data Types, Operators ── */
function ex2Demo() {
  const eventName = "Street Fair";
  const eventDate = "2025-08-15";
  let seats = 20;

  var info = `Event: ${eventName} | Date: ${eventDate} | Seats: ${seats}`;

  // simulate one registration
  seats--;
  var after = `After 1 registration – Seats left: ${seats}`;

  document.getElementById("ex2Output").innerHTML =
    "<p>" + info + "</p><p>" + after + "</p>";
}


/* ── Exercise 3: Conditionals, Loops, Error Handling ── */
function ex3Demo() {
  var html = "";

  events.forEach(function (ev) {
    if (ev.past) {
      html += "<p style='color:#aaa;'>[Past] " + ev.name + " – not shown</p>";
      return;
    }
    if (ev.seats === 0) {
      html += "<p style='color:#d93025;'>[Full] " + ev.name + "</p>";
      return;
    }
    html += "<p style='color:green;'>[Available] " + ev.name +
            " – " + ev.seats + " seats</p>";
  });

  document.getElementById("ex3Output").innerHTML = html;

  // try-catch around registration
  try {
    registerWithValidation({ name: "Street Fair", seats: 0 });
  } catch (err) {
    console.error("Registration error caught:", err.message);
  }
}

function registerWithValidation(ev) {
  if (ev.seats <= 0) {
    throw new Error("Cannot register – event is full: " + ev.name);
  }
  ev.seats--;
  console.log("Registered for", ev.name, "– seats left:", ev.seats);
}


/* ── Exercise 4: Functions, Scope, Closures, Higher-Order Functions ── */

function addEvent(list, ev) {
  list.push(ev);
}

function registerUser(eventName, list) {
  var ev = list.find(function (e) { return e.name === eventName; });
  if (!ev || ev.seats === 0) return "Registration failed for " + eventName;
  ev.seats--;
  return "Registered for " + eventName + ". Seats left: " + ev.seats;
}

function filterEventsByCategory(list, category, callback) {
  var filtered = list.filter(function (e) {
    return e.category === category && !e.past && e.seats > 0;
  });
  return callback(filtered);
}

// closure – tracks registration count per category
function makeRegistrationTracker(category) {
  var count = 0;
  return function () {
    count++;
    return category + " registrations so far: " + count;
  };
}

function ex4Demo() {
  var list = events.slice(); // copy

  // add a new event
  addEvent(list, { id: 7, name: "Pottery Class", date: "2025-11-01",
                   category: "workshop", seats: 10, past: false });

  var regMsg = registerUser("Street Fair", list);

  var musicEvents = filterEventsByCategory(list, "music", function (arr) {
    return arr.map(function (e) { return e.name; }).join(", ");
  });

  var trackMusic = makeRegistrationTracker("Music");
  var t1 = trackMusic();
  var t2 = trackMusic();

  document.getElementById("ex4Output").innerHTML =
    "<p>" + regMsg + "</p>" +
    "<p>Music events: " + musicEvents + "</p>" +
    "<p>" + t1 + "</p>" +
    "<p>" + t2 + "</p>";
}


/* ── Exercise 5: Objects and Prototypes ── */
function CommunityEvent(name, date, category, seats) {
  this.name     = name;
  this.date     = date;
  this.category = category;
  this.seats    = seats;
}

CommunityEvent.prototype.checkAvailability = function () {
  return this.seats > 0
    ? this.name + " is available (" + this.seats + " seats)"
    : this.name + " is full";
};

function ex5Demo() {
  var ev = new CommunityEvent("Street Fair", "2025-08-15", "food", 20);
  var html = "<p>" + ev.checkAvailability() + "</p>";
  html += "<p>Object entries:</p><ul>";

  Object.entries(ev).forEach(function (entry) {
    html += "<li><strong>" + entry[0] + "</strong>: " + entry[1] + "</li>";
  });

  html += "</ul>";
  document.getElementById("ex5Output").innerHTML = html;
}


/* ── Exercise 6: Arrays and Methods ── */
function ex6Demo() {
  var list = events.slice();

  // push
  list.push({ id: 8, name: "Drumming Workshop", date: "2025-11-15",
              category: "music", seats: 12, past: false });

  // filter – music only
  var musicOnly = list.filter(function (e) {
    return e.category === "music" && !e.past && e.seats > 0;
  });

  // map – format cards
  var cards = list.map(function (e) {
    return e.name.charAt(0).toUpperCase() + e.name.slice(1) +
           " (" + e.category + ")";
  });

  var html = "<p><strong>Music events:</strong> " +
             musicOnly.map(function (e) { return e.name; }).join(", ") + "</p>";
  html += "<p><strong>All formatted:</strong></p><ul>";
  cards.forEach(function (c) { html += "<li>" + c + "</li>"; });
  html += "</ul>";

  document.getElementById("ex6Output").innerHTML = html;
}


/* ── Exercise 7 & 8: DOM Manipulation and Event Handling ── */
function renderEvents(list) {
  var container = document.getElementById("eventList");
  container.innerHTML = "";

  list.forEach(function (ev) {
    if (ev.past || ev.seats === 0) return;

    var card = document.createElement("div");
    card.className = "event-card";
    card.setAttribute("data-id", ev.id);
    card.setAttribute("data-category", ev.category);

    card.innerHTML =
      "<span><strong>" + ev.name + "</strong> &nbsp; " + ev.date + "</span>" +
      "<span class='seats' id='seats-" + ev.id + "'>" + ev.seats + " seats</span>" +
      "<button onclick='registerFromCard(" + ev.id + ")'>Register</button>";

    container.appendChild(card);
  });
}

function registerFromCard(id) {
  var ev = events.find(function (e) { return e.id === id; });
  if (!ev || ev.seats === 0) return;
  ev.seats--;

  var seatsEl = document.querySelector("#seats-" + id);
  if (seatsEl) seatsEl.textContent = ev.seats + " seats";

  if (ev.seats === 0) {
    var card = document.querySelector("[data-id='" + id + "']");
    if (card) card.style.opacity = "0.4";
  }
}

function filterByCategory() {
  var val = document.getElementById("categoryFilter").value;
  var filtered = val === "all"
    ? events
    : events.filter(function (e) { return e.category === val; });
  renderEvents(filtered);
}

// Exercise 8 – keydown search
function searchEvents(e) {
  var query = e.target.value.toLowerCase();
  var filtered = events.filter(function (ev) {
    return ev.name.toLowerCase().includes(query);
  });
  renderEvents(filtered);
}


/* ── Exercise 9: Async JS, Promises, Async/Await ── */

// mock fetch using a resolved promise
function mockFetch() {
  return new Promise(function (resolve) {
    setTimeout(function () {
      resolve([
        { name: "Mock Street Fair",  category: "food",  seats: 40 },
        { name: "Mock Jazz Night",   category: "music", seats: 18 }
      ]);
    }, 1000);
  });
}

// using .then() / .catch()
function loadWithPromise() {
  document.getElementById("spinner").style.display = "block";
  document.getElementById("asyncResult").innerHTML = "";

  mockFetch()
    .then(function (data) {
      document.getElementById("spinner").style.display = "none";
      var html = "<p><strong>Loaded via .then():</strong></p><ul>";
      data.forEach(function (e) {
        html += "<li>" + e.name + " – " + e.seats + " seats</li>";
      });
      html += "</ul>";
      document.getElementById("asyncResult").innerHTML = html;
    })
    .catch(function (err) {
      document.getElementById("spinner").style.display = "none";
      document.getElementById("asyncResult").textContent = "Error: " + err;
    });
}

// using async/await
async function loadWithAsync() {
  document.getElementById("spinner").style.display = "block";
  document.getElementById("asyncResult").innerHTML = "";

  try {
    var data = await mockFetch();
    document.getElementById("spinner").style.display = "none";
    var html = "<p><strong>Loaded via async/await:</strong></p><ul>";
    data.forEach(function (e) {
      html += "<li>" + e.name + " – " + e.seats + " seats</li>";
    });
    html += "</ul>";
    document.getElementById("asyncResult").innerHTML = html;
  } catch (err) {
    document.getElementById("spinner").style.display = "none";
    document.getElementById("asyncResult").textContent = "Error: " + err;
  }
}


/* ── Exercise 10: Modern JS Features ── */
function ex10Demo() {
  // default parameter
  function formatEvent(name, category = "general") {
    return `${name} [${category}]`;
  }

  // destructuring
  const { name, date, seats } = events[0];
  const summary = `Name: ${name} | Date: ${date} | Seats: ${seats}`;

  // spread – clone before filtering so original is unchanged
  const cloned = [...events];
  const musicClone = cloned.filter(e => e.category === "music" && !e.past && e.seats > 0);

  document.getElementById("ex10Output").innerHTML =
    "<p>" + formatEvent("Street Fair", "food") + "</p>" +
    "<p>Destructured: " + summary + "</p>" +
    "<p>Music events (from spread clone): " +
    musicClone.map(e => e.name).join(", ") + "</p>";
}


/* ── Exercise 11: Working with Forms ── */
function handleRegForm(e) {
  e.preventDefault();

  var form  = e.target;
  var name  = form.elements["regName"].value.trim();
  var email = form.elements["regEmail"].value.trim();
  var ev    = form.elements["regEvent"].value;

  var valid = true;

  document.getElementById("nameErr").textContent  = "";
  document.getElementById("emailErr").textContent = "";
  document.getElementById("eventErr").textContent = "";
  document.getElementById("formMsg").textContent  = "";

  if (!name) {
    document.getElementById("nameErr").textContent = " Name is required.";
    valid = false;
  }
  if (!email || !email.includes("@")) {
    document.getElementById("emailErr").textContent = " Valid email required.";
    valid = false;
  }
  if (!ev) {
    document.getElementById("eventErr").textContent = " Please select an event.";
    valid = false;
  }

  if (valid) {
    console.log("Form submitted:", { name, email, event: ev });
    document.getElementById("formMsg").style.color = "green";
    document.getElementById("formMsg").textContent =
      "Registered! " + name + " → " + ev;
    form.reset();
  }
}


/* ── Exercise 12: AJAX & Fetch API (POST) ── */
function submitViaFetch() {
  var msg = document.getElementById("fetchMsg");
  msg.textContent = "Sending...";

  // simulate delayed response with setTimeout
  setTimeout(function () {
    fetch("https://jsonplaceholder.typicode.com/posts", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        name: "Test User",
        email: "test@portal.com",
        event: "Street Fair"
      })
    })
      .then(function (res) {
        if (!res.ok) throw new Error("Server error: " + res.status);
        return res.json();
      })
      .then(function (data) {
        console.log("POST response:", data);
        msg.style.color = "green";
        msg.textContent = "Submitted successfully! (mock id: " + data.id + ")";
      })
      .catch(function (err) {
        msg.style.color = "red";
        msg.textContent = "Submission failed: " + err.message;
      });
  }, 1500);
}


/* ── Exercise 13: Debugging ── */
function debugStep() {
  var name  = "Debug User";
  var event = "Street Fair";
  var seats = 20;

  // set a breakpoint on the next line in DevTools → Sources
  console.log("Step 1 – name:", name);
  console.log("Step 2 – event:", event);
  console.log("Step 3 – seats before:", seats);
  seats--;
  console.log("Step 4 – seats after registration:", seats);
  console.log("Step 5 – fetch payload would be:", JSON.stringify({ name, event }));
  alert("Debug steps logged to Console.");
}


/* ── Exercise 14: jQuery ── */
$(document).ready(function () {
  $("#registerBtn").click(function () {
    $("#jqMsg").fadeIn(400);
    setTimeout(function () { $("#jqMsg").fadeOut(600); }, 2000);
  });
});


/* ── Initialise event list on DOM ready ── */
document.addEventListener("DOMContentLoaded", function () {
  renderEvents(events);
});
