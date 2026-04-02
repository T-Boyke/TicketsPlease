/**
 * Markdown & Mermaid Engine for TicketsPlease
 * Uses marked.js for Markdown, Mermaid.js for diagrams, and DOMPurify for security.
 */

window.MarkdownEngine = {
    renderAll: function() {
        const elements = document.querySelectorAll('.markdown-content');
        elements.forEach(el => {
            if (el.dataset.rendered === 'true') return;
            
            const rawContent = el.textContent.trim();
            if (!rawContent) return;

            // Render Markdown
            let html = marked.parse(rawContent);
            
            // Sanitize HTML
            html = DOMPurify.sanitize(html);
            
            el.innerHTML = html;
            el.dataset.rendered = 'true';
            el.classList.remove('invisible'); // Show after render
        });

        // Render Mermaid Diagrams if mermaid is available
        if (window.mermaid) {
            mermaid.run({
                nodes: document.querySelectorAll('.mermaid'),
            });
        }
    }
};

// Auto-run on page load
document.addEventListener('DOMContentLoaded', () => {
    // Configure marked for Mermaid support
    marked.setOptions({
        highlight: function(code, lang) {
            if (lang === 'mermaid') {
                return `<pre class="mermaid">${code}</pre>`;
            }
            return code;
        }
    });

    window.MarkdownEngine.renderAll();
});
