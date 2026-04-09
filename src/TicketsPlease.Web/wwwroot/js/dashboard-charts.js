/**
 * TicketsPlease Dashboard Performance Charts
 * Logic for highscore tabs, API details, and Chart.js rendering.
 */

let statusChart = null;
let priorityChart = null;

/**
 * Switches between individual and team highscore panels.
 * @param {string} type - 'individual' or 'teams'
 */
function switchHighscore(type) {
    const panels = document.querySelectorAll('.highscore-panel');
    const buttons = document.querySelectorAll('.highscore-tab-btn');

    panels.forEach(p => p.classList.add('hidden'));
    buttons.forEach(b => b.classList.remove('active'));

    document.getElementById(`highscore-${type}`).classList.remove('hidden');
    document.getElementById(`btn-${type}`).classList.add('active');
}

/**
 * Fetches performance data from the API and opens the detail modal.
 * @param {string} type - 'user' or 'team'
 * @param {string} id - The GUID of the entity
 */
async function openPerformanceDetail(type, id) {
    const modal = document.getElementById('performance-modal');
    modal.classList.remove('hidden');
    document.body.style.overflow = 'hidden';

    // Reset view
    document.getElementById('detail-name').innerText = 'Loading...';
    document.getElementById('detail-hours').innerText = '0.0h';
    document.getElementById('detail-tickets').innerText = '0';
    document.getElementById('detail-points').innerText = '0';

    try {
        const response = await fetch(`/api/dashboard/${type}/${id}`);
        if (!response.ok) throw new Error('Failed to fetch data');

        const data = await response.json();

        // Update Text
        document.getElementById('detail-name').innerText = data.name;
        document.getElementById('detail-hours').innerText = `${data.totalHours.toFixed(1)}h`;
        document.getElementById('detail-tickets').innerText = data.totalTickets;
        document.getElementById('detail-points').innerText = data.totalPoints;

        // Render Charts
        renderStatusChart(data.statusDistribution);
        renderPriorityChart(data.priorityDistribution);

    } catch (error) {
        console.error('Error fetching performance details:', error);
        document.getElementById('detail-name').innerText = 'Error loading data';
    }
}

/**
 * Closes the modal and resets charts.
 */
function closePerformanceModal() {
    const modal = document.getElementById('performance-modal');
    modal.classList.add('hidden');
    document.body.style.overflow = '';
}

/**
 * Renders the Status Distribution Pie Chart.
 */
function renderStatusChart(distribution) {
    const ctx = document.getElementById('chart-status').getContext('2d');
    
    if (statusChart) statusChart.destroy();

    const labels = Object.keys(distribution);
    const values = Object.values(distribution);

    statusChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: [
                    '#3b82f6', // blue
                    '#10b981', // emerald
                    '#f59e0b', // amber
                    '#ef4444', // rose
                    '#8b5cf6', // violet
                    '#64748b'  // slate
                ],
                borderWidth: 0,
                hoverOffset: 15
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        usePointStyle: true,
                        padding: 20,
                        font: { size: 10, weight: 'bold' }
                    }
                }
            },
            cutout: '70%'
        }
    });
}

/**
 * Renders the Priority Distribution Pie Chart.
 */
function renderPriorityChart(distribution) {
    const ctx = document.getElementById('chart-priority').getContext('2d');
    
    if (priorityChart) priorityChart.destroy();

    const labels = Object.keys(distribution);
    const values = Object.values(distribution);

    priorityChart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: [
                    '#ef4444', // critical/blocker
                    '#f59e0b', // high
                    '#3b82f6', // medium
                    '#94a3b8'  // low
                ],
                borderWidth: 2,
                borderColor: '#ffffff'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        usePointStyle: true,
                        padding: 20,
                        font: { size: 10, weight: 'bold' }
                    }
                }
            }
        }
    });
}
