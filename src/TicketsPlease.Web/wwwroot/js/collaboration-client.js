/**
 * TicketsPlease Collaboration Client
 * Handles real-time presence and team chat.
 */

const collabConnection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .withAutomaticReconnect()
    .build();

const chatWindow = document.getElementById('chat-window');
const chatMessages = document.getElementById('chat-messages');
const chatInput = document.getElementById('chat-input');
const chatForm = document.getElementById('chat-form');
const onlineUsersList = document.getElementById('online-users-list');
const chatIconOpen = document.getElementById('chat-icon-open');
const chatIconClose = document.getElementById('chat-icon-close');
const chatNewBadge = document.getElementById('chat-new-badge');

let isChatOpen = false;

function toggleChat() {
    isChatOpen = !isChatOpen;
    if (isChatOpen) {
        chatWindow.classList.add('active');
        if (chatIconOpen) chatIconOpen.classList.add('translate-y-16');
        if (chatIconClose) chatIconClose.classList.remove('translate-y-16');
        if (chatNewBadge) chatNewBadge.classList.add('hidden');
        loadChatHistory();
        setTimeout(() => {
            chatMessages.scrollTop = chatMessages.scrollHeight;
        }, 100);
    } else {
        chatWindow.classList.remove('active');
        if (chatIconOpen) chatIconOpen.classList.remove('translate-y-16');
        if (chatIconClose) chatIconClose.classList.add('translate-y-16');
    }
}

async function loadChatHistory() {
    try {
        const response = await fetch('/Chat/GetGlobalHistory');
        const messages = await response.json();
        renderMessages(messages);
    } catch (err) {
        console.error('Failed to load chat history', err);
    }
}

function renderMessage(msg, isHistory = false) {
    const isMine = msg.senderUserName === (window.currentUserName || '');
    const messageDiv = document.createElement('div');
    messageDiv.className = `flex flex-col ${isMine ? 'items-end' : 'items-start'} mb-2 group animate-in slide-in-from-bottom-2 duration-300`;

    const time = new Date(msg.sentAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

    messageDiv.innerHTML = `
        <div class="flex items-center space-x-2 mb-1">
            ${!isMine ? `<span class="text-[10px] font-bold text-slate-500">${msg.senderUserName}</span>` : ''}
            <span class="text-[9px] text-slate-400 opacity-0 group-hover:opacity-100 transition-opacity">${time}</span>
        </div>
        <div class="max-w-[85%] px-4 py-2 ${isMine ? 'bg-brand-primary text-white rounded-l-2xl rounded-tr-2xl' : 'bg-slate-100 text-slate-700 rounded-r-2xl rounded-tl-2xl'} text-sm shadow-sm">
            ${msg.bodyMarkdown}
        </div>
    `;

    if (!isHistory && chatMessages.querySelector('.opacity-30')) {
        chatMessages.innerHTML = '';
    }

    chatMessages.appendChild(messageDiv);
    if (!isHistory) {
        chatMessages.scrollTop = chatMessages.scrollHeight;
        if (!isChatOpen) {
            chatNewBadge.classList.remove('hidden');
        }
    }
}

function renderMessages(messages) {
    chatMessages.innerHTML = '';
    messages.forEach(m => renderMessage(m, true));
    chatMessages.scrollTop = chatMessages.scrollHeight;
}

// Presence Logic
collabConnection.on("UserPresenceChanged", function (usernames) {
    onlineUsersList.innerHTML = '';
    usernames.forEach(user => {
        const initials = user.substring(0, 2).toUpperCase();
        const avatar = document.createElement('div');
        avatar.className = "h-7 w-7 rounded-full bg-brand-primary border-2 border-white flex items-center justify-center text-[10px] font-black text-white shadow-sm ring-1 ring-slate-100";
        avatar.title = user;
        avatar.textContent = initials;
        onlineUsersList.appendChild(avatar);
    });
});

// Chat Logic
collabConnection.on("ReceiveGlobalMessage", function (message) {
    renderMessage(message);
});

chatForm.onsubmit = async (e) => {
    e.preventDefault();
    const text = chatInput.value.trim();
    if (!text) return;

    chatInput.value = '';

    const formData = new FormData();
    formData.append('BodyMarkdown', text);

    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    try {
        const response = await fetch('/Chat/SendMessage', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': token
            }
        });
        const msg = await response.json();

        // Broadcast via SignalR
        await collabConnection.invoke("SendGlobalMessage", msg);
    } catch (err) {
        console.error('Failed to send message', err);
    }
};

collabConnection.start()
    .then(() => {
        console.log("Collaboration connection started");
        collabConnection.invoke("JoinGlobalGroup");
    })
    .catch(err => console.error(err.toString()));

// Auto-expand textarea
chatInput.oninput = () => {
    chatInput.style.height = 'auto';
    chatInput.style.height = (chatInput.scrollHeight) + 'px';
};
