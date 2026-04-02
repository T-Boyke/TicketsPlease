/**
 * SignalR Client für Echtzeit-Kollaboration und Benachrichtigungen.
 * (BitLC-NE-2025-2026 Enterprise Phase 2)
 */

"use strict";

const notificationConnection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .withAutomaticReconnect()
    .build();

notificationConnection.on("ReceiveNotification", (notification) => {
    // In-App Toast anzeigen
    console.log("Notification received:", notification);
    showToast(notification.title, notification.message, notification.link);
});

notificationConnection.on("ReceiveGlobalAlert", (alert) => {
    // Globale System-Meldung (z.B. Wartungsarbeiten)
    console.log("Global alert:", alert);
    showGlobalAlert(alert.title, alert.message);
});

notificationConnection.on("TicketUpdated", (data) => {
    // Falls wir auf dem Kanban-Board sind, neu laden oder Karte updaten
    console.log("Ticket updated:", data);
    
    if (window.location.pathname.toLowerCase().includes("/tickets")) {
        // Falls speziell die Ticket-Index (Board) oder Details
        const board = document.getElementById("kanban-board");
        if (board) {
            // Animiertes Feedback: "Daten nicht mehr aktuell" oder automatischer Refresh
            showToast("Aktualisierung", data.message, null);
            // Optional: loadKanbanData(); // Wenn wir eine Refresh-Funktion haben
        }
        
        const detailsContainer = document.getElementById("ticket-details-container");
        if (detailsContainer && detailsContainer.dataset.ticketId === data.ticketId) {
             showToast("Änderung", "Dieses Ticket wurde gerade aktualisiert.", null);
        }
    }
});

notificationConnection.on("PresenceUpdated", (users) => {
    console.log("Users viewing this ticket:", users);
    updatePresenceContainer(users);
});

notificationConnection.on("NewComment", (data) => {
    console.log("New comment received:", data);
    const commentList = document.getElementById("comments-list");
    if (commentList && commentList.dataset.ticketId === data.ticketId) {
        appendComment(data.comment);
        showToast("Neuer Kommentar", `${data.comment.authorName} hat kommentiert.`, null);
    }
});

async function startSignalR() {
    try {
        await notificationConnection.start();
        console.log("SignalR connected.");
        
        // Falls wir auf einer Ticket-Detail-Seite sind, Gruppe beitreten
        const detailsContainer = document.getElementById("ticket-details-container");
        if (detailsContainer && detailsContainer.dataset.ticketId) {
            await notificationConnection.invoke("JoinTicketGroup", detailsContainer.dataset.ticketId);
        }
    } catch (err) {
        console.error("SignalR connection error:", err);
        setTimeout(startSignalR, 5000);
    }
}

function showToast(title, message, link) {
    // Einfache Enterprise-Toast-Implementierung (kann durch Bootstrap/Library ersetzt werden)
    const toastContainer = document.getElementById("enterprise-toast-container");
    if (!toastContainer) return;

    const toast = document.createElement("div");
    toast.className = "enterprise-toast show animate-slide-in";
    toast.innerHTML = `
        <div class="toast-header">
            <strong>${title}</strong>
            <button type="button" class="btn-close" onclick="this.parentElement.parentElement.remove()"></button>
        </div>
        <div class="toast-body">
            ${message}
            ${link ? `<br><a href="${link}" class="btn btn-sm btn-link">Ansehen</a>` : ''}
        </div>
    `;
    toastContainer.appendChild(toast);
    setTimeout(() => { if (toast) toast.remove(); }, 10000);
}

function appendComment(comment) {
    const list = document.getElementById("comments-list");
    if (!list) return;

    const div = document.createElement("div");
    div.className = "comment-item animate-fade-in";
    div.innerHTML = `
        <div class="comment-header">
            <span class="comment-author">${comment.authorName}</span>
            <span class="comment-date">jetzt</span>
        </div>
        <div class="comment-body markdown-content">
            ${comment.content}
        </div>
    `;
    list.prepend(div);
    
    // Markdown-Engine triggern (falls vorhanden)
    if (window.renderMarkdown) {
        window.renderMarkdown(div.querySelector(".markdown-content"));
    }
}

function updatePresenceContainer(users) {
    const container = document.getElementById("ticket-presence-container");
    if (!container) return;

    container.innerHTML = '';
    users.forEach(user => {
        const badge = document.createElement("span");
        badge.className = "presence-badge animate-fade-in";
        badge.innerHTML = `<i class="fa-solid fa-circle text-emerald-500 mr-2 text-[6px]"></i> ${user}`;
        container.appendChild(badge);
    });
}

// Start beim Laden
document.addEventListener("DOMContentLoaded", startSignalR);
