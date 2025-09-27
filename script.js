// Learning Copilot - Course Registration Management System
class LearningCopilot {
    constructor() {
        this.currentUser = null;
        this.courses = [];
        this.userRegistrations = new Map();
        this.initializeData();
        this.bindEvents();
        this.checkLogin();
    }

    // Initialize sample data
    initializeData() {
        const savedCourses = localStorage.getItem('learningCopilot_courses');
        const savedRegistrations = localStorage.getItem('learningCopilot_registrations');
        const savedUser = localStorage.getItem('learningCopilot_currentUser');

        if (savedCourses) {
            this.courses = JSON.parse(savedCourses);
        } else {
            // Create sample courses
            this.courses = [
                {
                    id: 1,
                    title: "JavaScript Fundamentals",
                    description: "Impara le basi di JavaScript per lo sviluppo web moderno.",
                    duration: "8 settimane",
                    status: "active",
                    startDate: "2024-01-15",
                    endDate: "2024-03-15",
                    participants: []
                },
                {
                    id: 2,
                    title: "React Development",
                    description: "Costruisci applicazioni web dinamiche con React.",
                    duration: "10 settimane",
                    status: "active",
                    startDate: "2024-02-01",
                    endDate: "2024-04-15",
                    participants: []
                },
                {
                    id: 3,
                    title: "Python for Beginners",
                    description: "Introduzione alla programmazione con Python.",
                    duration: "6 settimane",
                    status: "active",
                    startDate: "2024-01-20",
                    endDate: "2024-03-05",
                    participants: []
                },
                {
                    id: 4,
                    title: "Web Design Basics",
                    description: "Principi fondamentali del design web e UX/UI.",
                    duration: "4 settimane",
                    status: "concluded",
                    startDate: "2023-11-01",
                    endDate: "2023-11-30",
                    participants: []
                },
                {
                    id: 5,
                    title: "Database Design",
                    description: "Progettazione e gestione di database relazionali.",
                    duration: "12 settimane",
                    status: "active",
                    startDate: "2024-01-10",
                    endDate: "2024-04-10",
                    participants: []
                }
            ];
            this.saveData();
        }

        if (savedRegistrations) {
            const registrationsArray = JSON.parse(savedRegistrations);
            this.userRegistrations = new Map(registrationsArray);
        }

        if (savedUser) {
            this.currentUser = savedUser;
        }
    }

    // Save data to localStorage
    saveData() {
        localStorage.setItem('learningCopilot_courses', JSON.stringify(this.courses));
        localStorage.setItem('learningCopilot_registrations', JSON.stringify([...this.userRegistrations]));
        if (this.currentUser) {
            localStorage.setItem('learningCopilot_currentUser', this.currentUser);
        }
    }

    // Check if user is logged in
    checkLogin() {
        if (this.currentUser) {
            this.showMainApp();
        } else {
            this.showLogin();
        }
    }

    // Bind event listeners
    bindEvents() {
        // Login
        document.getElementById('login-btn').addEventListener('click', () => this.login());
        document.getElementById('username').addEventListener('keypress', (e) => {
            if (e.key === 'Enter') this.login();
        });

        // Navigation
        document.getElementById('nav-courses').addEventListener('click', () => this.showCoursesSection());
        document.getElementById('nav-my-courses').addEventListener('click', () => this.showMyCoursesSection());
        document.getElementById('nav-profile').addEventListener('click', () => this.showProfileSection());

        // Profile
        document.getElementById('logout-btn').addEventListener('click', () => this.logout());

        // Browse courses button
        document.getElementById('browse-courses-btn').addEventListener('click', () => this.showCoursesSection());

        // Modal events
        document.getElementById('cancel-modal').addEventListener('click', () => this.hideModal());
        document.getElementById('confirm-cancel').addEventListener('click', () => this.confirmCancellation());

        // Close modal when clicking outside
        document.getElementById('confirmation-modal').addEventListener('click', (e) => {
            if (e.target.id === 'confirmation-modal') {
                this.hideModal();
            }
        });
    }

    // Login functionality
    login() {
        const username = document.getElementById('username').value.trim();
        if (username.length < 2) {
            this.showMessage('Inserisci un nome utente valido (almeno 2 caratteri)', 'error');
            return;
        }

        this.currentUser = username;
        this.saveData();
        this.showMessage(`Benvenuto, ${username}!`, 'success');
        this.showMainApp();
    }

    // Logout functionality
    logout() {
        this.currentUser = null;
        localStorage.removeItem('learningCopilot_currentUser');
        this.showLogin();
        this.showMessage('Logout effettuato con successo', 'info');
    }

    // Show login section
    showLogin() {
        document.getElementById('login-section').classList.remove('hidden');
        document.getElementById('courses-section').classList.add('hidden');
        document.getElementById('my-courses-section').classList.add('hidden');
        document.getElementById('profile-section').classList.add('hidden');
        document.querySelector('nav').style.display = 'none';
        document.getElementById('username').focus();
    }

    // Show main application
    showMainApp() {
        document.getElementById('login-section').classList.add('hidden');
        document.querySelector('nav').style.display = 'flex';
        this.showCoursesSection();
    }

    // Show courses section
    showCoursesSection() {
        this.setActiveNav('nav-courses');
        document.getElementById('courses-section').classList.remove('hidden');
        document.getElementById('my-courses-section').classList.add('hidden');
        document.getElementById('profile-section').classList.add('hidden');
        this.renderCourses();
    }

    // Show my courses section
    showMyCoursesSection() {
        this.setActiveNav('nav-my-courses');
        document.getElementById('courses-section').classList.add('hidden');
        document.getElementById('my-courses-section').classList.remove('hidden');
        document.getElementById('profile-section').classList.add('hidden');
        this.renderMyCourses();
    }

    // Show profile section
    showProfileSection() {
        this.setActiveNav('nav-profile');
        document.getElementById('courses-section').classList.add('hidden');
        document.getElementById('my-courses-section').classList.add('hidden');
        document.getElementById('profile-section').classList.remove('hidden');
        this.renderProfile();
    }

    // Set active navigation button
    setActiveNav(activeId) {
        document.querySelectorAll('.nav-btn').forEach(btn => btn.classList.remove('active'));
        document.getElementById(activeId).classList.add('active');
    }

    // Get user's registered courses
    getUserRegistrations() {
        return this.userRegistrations.get(this.currentUser) || [];
    }

    // Check if user is registered for a course
    isRegistered(courseId) {
        const registrations = this.getUserRegistrations();
        return registrations.includes(courseId);
    }

    // Register for a course
    registerForCourse(courseId) {
        const course = this.courses.find(c => c.id === courseId);
        if (!course) return;

        if (course.status === 'concluded') {
            this.showMessage('Non è possibile iscriversi a un corso già concluso', 'error');
            return;
        }

        if (this.isRegistered(courseId)) {
            this.showMessage('Sei già iscritto a questo corso', 'info');
            return;
        }

        const registrations = this.getUserRegistrations();
        registrations.push(courseId);
        this.userRegistrations.set(this.currentUser, registrations);

        // Add user to course participants
        if (!course.participants.includes(this.currentUser)) {
            course.participants.push(this.currentUser);
        }

        this.saveData();
        this.showMessage(`Ti sei iscritto con successo al corso "${course.title}"`, 'success');
        this.renderCourses();
        this.updateProfileCourseCount();
    }

    // Cancel course registration
    cancelRegistration(courseId) {
        const course = this.courses.find(c => c.id === courseId);
        if (!course) return;

        if (course.status === 'concluded') {
            this.showMessage('Non è possibile cancellare l\'iscrizione da un corso già concluso', 'error');
            return;
        }

        // Show confirmation modal
        const modal = document.getElementById('confirmation-modal');
        const message = document.getElementById('confirmation-message');
        message.textContent = `Sei sicuro di voler cancellare la tua iscrizione dal corso "${course.title}"?`;
        
        // Store the course ID for the confirmation
        modal.dataset.courseId = courseId;
        modal.classList.remove('hidden');
    }

    // Confirm cancellation
    confirmCancellation() {
        const modal = document.getElementById('confirmation-modal');
        const courseId = parseInt(modal.dataset.courseId);
        
        this.hideModal();
        this.performCancellation(courseId);
    }

    // Perform the actual cancellation
    performCancellation(courseId) {
        const course = this.courses.find(c => c.id === courseId);
        if (!course) return;

        const registrations = this.getUserRegistrations();
        const index = registrations.indexOf(courseId);
        
        if (index === -1) {
            this.showMessage('Non sei iscritto a questo corso', 'error');
            return;
        }

        // Remove from user registrations
        registrations.splice(index, 1);
        this.userRegistrations.set(this.currentUser, registrations);

        // Remove user from course participants
        const participantIndex = course.participants.indexOf(this.currentUser);
        if (participantIndex !== -1) {
            course.participants.splice(participantIndex, 1);
        }

        this.saveData();
        this.showMessage(`Hai cancellato con successo l'iscrizione dal corso "${course.title}"`, 'success');
        
        // Refresh the current view
        if (!document.getElementById('my-courses-section').classList.contains('hidden')) {
            this.renderMyCourses();
        } else if (!document.getElementById('courses-section').classList.contains('hidden')) {
            this.renderCourses();
        }
        
        this.updateProfileCourseCount();
    }

    // Hide modal
    hideModal() {
        document.getElementById('confirmation-modal').classList.add('hidden');
    }

    // Render available courses
    renderCourses() {
        const container = document.getElementById('courses-list');
        container.innerHTML = '';

        this.courses.forEach(course => {
            const courseCard = document.createElement('div');
            courseCard.className = 'course-card';
            
            const isRegistered = this.isRegistered(course.id);
            const statusClass = course.status === 'concluded' ? 'concluded' : 'active';
            
            courseCard.innerHTML = `
                <h3>${course.title}</h3>
                <p>${course.description}</p>
                <div class="course-info">
                    <span>Durata: ${course.duration}</span>
                    <span class="course-status ${statusClass}">
                        ${course.status === 'concluded' ? 'Concluso' : 'Attivo'}
                    </span>
                </div>
                <div class="participants-count">
                    Partecipanti: ${course.participants.length}
                </div>
                <div class="course-actions">
                    ${isRegistered 
                        ? `<button class="btn btn-danger" onclick="app.cancelRegistration(${course.id})">
                             ${course.status === 'concluded' ? 'Iscrizione Conclusa' : 'Cancella Iscrizione'}
                           </button>`
                        : `<button class="btn btn-success" onclick="app.registerForCourse(${course.id})" 
                             ${course.status === 'concluded' ? 'disabled' : ''}>
                             ${course.status === 'concluded' ? 'Corso Concluso' : 'Iscriviti'}
                           </button>`
                    }
                </div>
            `;

            container.appendChild(courseCard);
        });
    }

    // Render user's courses
    renderMyCourses() {
        const container = document.getElementById('my-courses-list');
        const noCoursesMessage = document.getElementById('no-courses-message');
        
        const userRegistrations = this.getUserRegistrations();
        const userCourses = this.courses.filter(course => userRegistrations.includes(course.id));

        container.innerHTML = '';

        if (userCourses.length === 0) {
            container.classList.add('hidden');
            noCoursesMessage.classList.remove('hidden');
            return;
        }

        container.classList.remove('hidden');
        noCoursesMessage.classList.add('hidden');

        userCourses.forEach(course => {
            const courseCard = document.createElement('div');
            courseCard.className = 'course-card';
            
            const statusClass = course.status === 'concluded' ? 'concluded' : 'active';
            const canCancel = course.status !== 'concluded';
            
            courseCard.innerHTML = `
                <h3>${course.title}</h3>
                <p>${course.description}</p>
                <div class="course-info">
                    <span>Durata: ${course.duration}</span>
                    <span class="course-status ${statusClass}">
                        ${course.status === 'concluded' ? 'Concluso' : 'In Corso'}
                    </span>
                </div>
                <div class="course-info">
                    <span>Inizio: ${new Date(course.startDate).toLocaleDateString('it-IT')}</span>
                    <span>Fine: ${new Date(course.endDate).toLocaleDateString('it-IT')}</span>
                </div>
                <div class="participants-count">
                    Altri partecipanti: ${course.participants.length - 1}
                </div>
                <div class="course-actions">
                    <button class="btn ${canCancel ? 'btn-danger' : 'btn-secondary'}" 
                            onclick="app.cancelRegistration(${course.id})"
                            ${!canCancel ? 'disabled' : ''}>
                        ${canCancel ? 'Cancella Iscrizione' : 'Corso Concluso'}
                    </button>
                </div>
            `;

            container.appendChild(courseCard);
        });
    }

    // Render profile information
    renderProfile() {
        document.getElementById('profile-username').textContent = this.currentUser;
        this.updateProfileCourseCount();
    }

    // Update profile course count
    updateProfileCourseCount() {
        const count = this.getUserRegistrations().length;
        const profileCount = document.getElementById('profile-course-count');
        if (profileCount) {
            profileCount.textContent = count;
        }
    }

    // Show messages
    showMessage(text, type = 'info') {
        const container = document.getElementById('message-container');
        const message = document.createElement('div');
        message.className = `message message-${type}`;
        message.textContent = text;

        container.appendChild(message);

        // Auto-remove message after 5 seconds
        setTimeout(() => {
            if (message.parentNode) {
                message.parentNode.removeChild(message);
            }
        }, 5000);
    }

    // Get course statistics (for potential future use)
    getCourseStats(courseId) {
        const course = this.courses.find(c => c.id === courseId);
        if (!course) return null;

        return {
            id: course.id,
            title: course.title,
            participantCount: course.participants.length,
            status: course.status,
            isActive: course.status === 'active'
        };
    }

    // Get user statistics
    getUserStats() {
        const registrations = this.getUserRegistrations();
        const activeCourses = registrations.filter(courseId => {
            const course = this.courses.find(c => c.id === courseId);
            return course && course.status === 'active';
        });
        const concludedCourses = registrations.filter(courseId => {
            const course = this.courses.find(c => c.id === courseId);
            return course && course.status === 'concluded';
        });

        return {
            totalRegistrations: registrations.length,
            activeCourses: activeCourses.length,
            concludedCourses: concludedCourses.length
        };
    }
}

// Initialize the application
const app = new LearningCopilot();

// Make app globally accessible for onclick handlers
window.app = app;