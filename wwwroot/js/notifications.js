/**
 * StudyHub - Notification Center & Notification Bar Engine
 * Role-Aware, LocalStorage-Synchronized Client Notification Architecture
 */

(function () {
    'use strict';

    window.StudyHub = window.StudyHub || {};

    const STORAGE_KEY = 'studyhub_notifications_store_v1';
    const SELECTED_ROLE_KEY = 'studyhub_selected_role_v1';

    // --- 1. Default Mock Data Sets By Role ---
    const DEFAULT_NOTIFICATIONS = {
        Student: [
            {
                id: 'notif-stu-1',
                title: 'Welcome to StudyHub!',
                message: 'Your academic workspace is ready. Explore verified past papers, recorded video lectures, and student notes.',
                timestamp: 'Just now',
                createdAt: Date.now() - 1000 * 60 * 2, // 2 mins ago
                category: 'Academic',
                type: 'academic',
                icon: '🎓',
                isRead: false,
                link: '/Home/PastPapers',
                actionLabel: 'Explore Past Papers',
                role: 'Student'
            },
            {
                id: 'notif-stu-2',
                title: 'New CSE-201 Past Paper Uploaded',
                message: 'Spring 2025 Mid-Term examination questions with verified answer keys have been published by Class Moderator.',
                timestamp: '25 mins ago',
                createdAt: Date.now() - 1000 * 60 * 25,
                category: 'Resource',
                type: 'resource',
                icon: '📄',
                isRead: false,
                link: '/Home/PastPapers',
                actionLabel: 'View Paper',
                role: 'Student'
            },
            {
                id: 'notif-stu-3',
                title: 'Daily Focus Goal Reminder',
                message: 'Your daily focus goal "Complete 45-min Algorithm Review" is active for today.',
                timestamp: '2 hours ago',
                createdAt: Date.now() - 1000 * 60 * 120,
                category: 'Alert',
                type: 'alert',
                icon: '⚡',
                isRead: false,
                link: '/',
                actionLabel: 'Open Dashboard',
                role: 'Student'
            },
            {
                id: 'notif-stu-4',
                title: 'Student Account Verified',
                message: 'Your institutional university email has been verified. You have full access to high-speed downloads.',
                timestamp: 'Yesterday',
                createdAt: Date.now() - 1000 * 60 * 60 * 26,
                category: 'System',
                type: 'system',
                icon: '✓',
                isRead: true,
                link: '/Account/Profile',
                actionLabel: 'View Profile',
                role: 'Student'
            },
            {
                id: 'notif-stu-5',
                title: 'New Lecture Recording: Database Normalization',
                message: 'Prof. Rahman published video lecture and slide handouts for CSE-305 Section A.',
                timestamp: '2 days ago',
                createdAt: Date.now() - 1000 * 60 * 60 * 48,
                category: 'Lecture',
                type: 'resource',
                icon: '🎥',
                isRead: true,
                link: '/Home/Lectures',
                actionLabel: 'Watch Lecture',
                role: 'Student'
            }
        ],
        CR: [
            {
                id: 'notif-cr-1',
                title: 'Batch Material Request (3 Pending)',
                message: 'Students from Section B requested Spring 2025 Data Structures and Algorithms past solution sets.',
                timestamp: 'Just now',
                createdAt: Date.now() - 1000 * 60 * 5,
                category: 'Academic',
                type: 'academic',
                icon: '📢',
                isRead: false,
                link: '/Home/PastPapers',
                actionLabel: 'Review Request',
                role: 'CR'
            },
            {
                id: 'notif-cr-2',
                title: 'Broadcast Announcement Delivered',
                message: 'Your announcement "Lab Final Schedule & Syllabus Breakdown" was delivered to 48 students.',
                timestamp: '40 mins ago',
                createdAt: Date.now() - 1000 * 60 * 40,
                category: 'System',
                type: 'system',
                icon: '✉️',
                isRead: false,
                link: '/',
                actionLabel: 'View Stats',
                role: 'CR'
            },
            {
                id: 'notif-cr-3',
                title: 'Pending Note Moderation',
                message: 'Student "Alex Morgan" uploaded notes for "Operating Systems Threading" awaiting your verification.',
                timestamp: '3 hours ago',
                createdAt: Date.now() - 1000 * 60 * 180,
                category: 'Moderation',
                type: 'moderation',
                icon: '🛡️',
                isRead: false,
                link: '/Home/Notes',
                actionLabel: 'Moderate Notes',
                role: 'CR'
            },
            {
                id: 'notif-cr-4',
                title: 'CR Access Renewed',
                message: 'Your Class Representative badge and publication privileges have been renewed for the current semester.',
                timestamp: 'Yesterday',
                createdAt: Date.now() - 1000 * 60 * 60 * 28,
                category: 'Profile',
                type: 'profile',
                icon: '⭐',
                isRead: true,
                link: '/Account/Profile',
                actionLabel: 'Profile Settings',
                role: 'CR'
            }
        ],
        Admin: [
            {
                id: 'notif-adm-1',
                title: 'System Security Audit Completed',
                message: 'Automated database backup and authentication audit completed with zero vulnerabilities found.',
                timestamp: 'Just now',
                createdAt: Date.now() - 1000 * 60 * 3,
                category: 'System',
                type: 'system',
                icon: '🔒',
                isRead: false,
                link: '/',
                actionLabel: 'System Status',
                role: 'Admin'
            },
            {
                id: 'notif-adm-2',
                title: 'New Moderator Approval Needed',
                message: 'User "Tasnia Mahek" submitted CR verification credentials for CSE Batch 2023.',
                timestamp: '1 hour ago',
                createdAt: Date.now() - 1000 * 60 * 60,
                category: 'Moderation',
                type: 'moderation',
                icon: '👤',
                isRead: false,
                link: '/Account/Profile',
                actionLabel: 'Review User',
                role: 'Admin'
            },
            {
                id: 'notif-adm-3',
                title: 'Cloud Repository Bandwidth Alert',
                message: 'Resource download volume reached 85% of monthly allocation threshold.',
                timestamp: '4 hours ago',
                createdAt: Date.now() - 1000 * 60 * 240,
                category: 'Alert',
                type: 'alert',
                icon: '⚠️',
                isRead: true,
                link: '/',
                actionLabel: 'Inspect Bandwidth',
                role: 'Admin'
            },
            {
                id: 'notif-adm-4',
                title: 'Weekly Portal Analytics Ready',
                message: '1,420 resource downloads and 380 active study sessions logged across university departments.',
                timestamp: 'Yesterday',
                createdAt: Date.now() - 1000 * 60 * 60 * 30,
                category: 'Report',
                type: 'academic',
                icon: '📊',
                isRead: true,
                link: '/',
                actionLabel: 'View Analytics',
                role: 'Admin'
            }
        ],
        Worker: [
            {
                id: 'notif-wrk-1',
                title: 'New Academic Tutoring Opportunity',
                message: 'A tutoring request for CSE-102 Introduction to Programming matches your verified profile.',
                timestamp: '10 mins ago',
                createdAt: Date.now() - 1000 * 60 * 10,
                category: 'Opportunity',
                type: 'academic',
                icon: '💼',
                isRead: false,
                link: '/Home/Notes',
                actionLabel: 'View Opportunity',
                role: 'Worker'
            },
            {
                id: 'notif-wrk-2',
                title: 'Application Status Updated',
                message: 'Your teaching assistant application for Spring Semester has moved to review stage.',
                timestamp: '1 hour ago',
                createdAt: Date.now() - 1000 * 60 * 60,
                category: 'System',
                type: 'system',
                icon: '📝',
                isRead: false,
                link: '/Account/Profile',
                actionLabel: 'Check Status',
                role: 'Worker'
            },
            {
                id: 'notif-wrk-3',
                title: 'Profile Skill Badge Verified',
                message: 'Your C++ and Data Structures certifications have been endorsed by the moderation team.',
                timestamp: 'Yesterday',
                createdAt: Date.now() - 1000 * 60 * 60 * 24,
                category: 'Profile',
                type: 'profile',
                icon: '🎖️',
                isRead: true,
                link: '/Account/Profile',
                actionLabel: 'View Badges',
                role: 'Worker'
            }
        ],
        Employer: [
            {
                id: 'notif-emp-1',
                title: '3 New Candidate Applications',
                message: 'Top students applied for your Campus Ambassador and Peer Tutor positions.',
                timestamp: '15 mins ago',
                createdAt: Date.now() - 1000 * 60 * 15,
                category: 'Opportunity',
                type: 'academic',
                icon: '👥',
                isRead: false,
                link: '/Account/Profile',
                actionLabel: 'Review Candidates',
                role: 'Employer'
            },
            {
                id: 'notif-emp-2',
                title: 'Tutor Interview Accepted',
                message: 'Candidate Alex Morgan accepted your interview invitation for Thursday 3:00 PM.',
                timestamp: '2 hours ago',
                createdAt: Date.now() - 1000 * 60 * 120,
                category: 'System',
                type: 'system',
                icon: '📅',
                isRead: false,
                link: '/Home/Notes',
                actionLabel: 'View Schedule',
                role: 'Employer'
            },
            {
                id: 'notif-emp-3',
                title: 'Organization Profile Live',
                message: 'Your verified partner badge is now visible to all registered students on StudyHub.',
                timestamp: '2 days ago',
                createdAt: Date.now() - 1000 * 60 * 60 * 48,
                category: 'Profile',
                type: 'profile',
                icon: '🏢',
                isRead: true,
                link: '/Account/Profile',
                actionLabel: 'Organization Profile',
                role: 'Employer'
            }
        ]
    };

    // --- 2. Notification Service Class ---
    class NotificationService {
        constructor() {
            this.initStore();
        }

        initStore() {
            const raw = localStorage.getItem(STORAGE_KEY);
            if (!raw) {
                this.saveStore(DEFAULT_NOTIFICATIONS);
            }
        }

        getStore() {
            try {
                const raw = localStorage.getItem(STORAGE_KEY);
                return raw ? JSON.parse(raw) : DEFAULT_NOTIFICATIONS;
            } catch (e) {
                console.error('Error loading notifications store:', e);
                return DEFAULT_NOTIFICATIONS;
            }
        }

        saveStore(data) {
            try {
                localStorage.setItem(STORAGE_KEY, JSON.stringify(data));
                this.dispatchUpdate();
            } catch (e) {
                console.error('Error saving notifications store:', e);
            }
        }

        getActiveRole() {
            const savedRole = localStorage.getItem(SELECTED_ROLE_KEY);
            if (savedRole && (DEFAULT_NOTIFICATIONS[savedRole])) {
                return savedRole;
            }
            const domRoleBadge = document.querySelector('.user-role-badge');
            if (domRoleBadge) {
                const roleText = domRoleBadge.textContent.trim();
                if (DEFAULT_NOTIFICATIONS[roleText]) return roleText;
            }
            return 'Student';
        }

        setActiveRole(role) {
            if (DEFAULT_NOTIFICATIONS[role]) {
                localStorage.setItem(SELECTED_ROLE_KEY, role);
                this.dispatchUpdate();
            }
        }

        getAll(role = this.getActiveRole()) {
            const store = this.getStore();
            return store[role] || [];
        }

        getUnread(role = this.getActiveRole()) {
            return this.getAll(role).filter(n => !n.isRead);
        }

        getUnreadCount(role = this.getActiveRole()) {
            return this.getUnread(role).length;
        }

        markAsRead(id, role = this.getActiveRole()) {
            const store = this.getStore();
            let changed = false;
            Object.keys(store).forEach(r => {
                const list = store[r];
                const notif = list.find(n => n.id === id);
                if (notif && !notif.isRead) {
                    notif.isRead = true;
                    changed = true;
                }
            });
            if (changed) {
                this.saveStore(store);
            }
        }

        markAsUnread(id, role = this.getActiveRole()) {
            const store = this.getStore();
            let changed = false;
            Object.keys(store).forEach(r => {
                const list = store[r];
                const notif = list.find(n => n.id === id);
                if (notif && notif.isRead) {
                    notif.isRead = false;
                    changed = true;
                }
            });
            if (changed) {
                this.saveStore(store);
            }
        }

        markAllAsRead(role = this.getActiveRole()) {
            const store = this.getStore();
            if (store[role]) {
                store[role].forEach(n => n.isRead = true);
                this.saveStore(store);
            }
        }

        deleteNotification(id) {
            const store = this.getStore();
            let changed = false;
            Object.keys(store).forEach(r => {
                const beforeLen = store[r].length;
                store[r] = store[r].filter(n => n.id !== id);
                if (store[r].length !== beforeLen) changed = true;
            });
            if (changed) {
                this.saveStore(store);
            }
        }

        clearAllRead(role = this.getActiveRole()) {
            const store = this.getStore();
            if (store[role]) {
                store[role] = store[role].filter(n => !n.isRead);
                this.saveStore(store);
            }
        }

        resetDefaults(role = this.getActiveRole()) {
            const store = this.getStore();
            if (DEFAULT_NOTIFICATIONS[role]) {
                store[role] = JSON.parse(JSON.stringify(DEFAULT_NOTIFICATIONS[role]));
                this.saveStore(store);
            }
        }

        resetAllRolesToDefaults() {
            this.saveStore(DEFAULT_NOTIFICATIONS);
        }

        addNotification(item, role = this.getActiveRole()) {
            const store = this.getStore();
            if (!store[role]) store[role] = [];
            const newItem = {
                id: 'notif-' + Date.now(),
                title: item.title || 'Notification',
                message: item.message || '',
                timestamp: 'Just now',
                createdAt: Date.now(),
                category: item.category || 'Academic',
                type: item.type || 'academic',
                icon: item.icon || '🔔',
                isRead: false,
                link: item.link || '',
                actionLabel: item.actionLabel || '',
                role: role
            };
            store[role].unshift(newItem);
            this.saveStore(store);
            return newItem;
        }

        dispatchUpdate() {
            document.dispatchEvent(new CustomEvent('studyhub:notifications-updated', {
                detail: {
                    activeRole: this.getActiveRole(),
                    unreadCount: this.getUnreadCount()
                }
            }));
        }
    }

    const service = new NotificationService();
    StudyHub.Notifications = service;

    // Helper: Map category/type to icon class
    function getCategoryIconClass(type) {
        switch ((type || '').toLowerCase()) {
            case 'academic': return 'icon-academic';
            case 'resource': case 'lecture': return 'icon-resource';
            case 'system': return 'icon-system';
            case 'alert': return 'icon-alert';
            case 'profile': return 'icon-profile';
            case 'moderation': return 'icon-moderation';
            default: return 'icon-academic';
        }
    }

    // --- 3. Navbar Bell & Dropdown UI Controller ---
    function initNavbarDropdown() {
        const bellBtn = document.querySelector('.notification-bell-btn');
        const dropdown = document.querySelector('.notification-dropdown');
        const badgeCount = document.querySelector('.notification-badge-count');
        const unreadPill = document.querySelector('.notif-unread-pill');
        const listBody = document.querySelector('.notification-dropdown-body');
        const markAllBtn = document.querySelector('.notif-mark-all-btn');
        const clearReadBtn = document.querySelector('.notif-clear-read-btn');
        const tabBtns = document.querySelectorAll('.notification-dropdown-tabs .notif-tab-btn');

        if (!bellBtn || !dropdown) return;

        let currentTabFilter = 'all';

        // Render Navbar Badge & Header Count
        function updateBadge() {
            const unread = service.getUnreadCount();
            if (badgeCount) {
                badgeCount.textContent = unread > 99 ? '99+' : unread;
                if (unread === 0) {
                    badgeCount.classList.add('is-hidden');
                } else {
                    badgeCount.classList.remove('is-hidden');
                }
            }
            if (unreadPill) {
                unreadPill.textContent = `${unread} new`;
                unreadPill.style.display = unread === 0 ? 'none' : '';
            }
        }

        // Render Dropdown List Items
        function renderDropdownList() {
            if (!listBody) return;
            const items = service.getAll();
            let filtered = items;

            if (currentTabFilter === 'unread') {
                filtered = items.filter(n => !n.isRead);
            } else if (currentTabFilter === 'academic') {
                filtered = items.filter(n => n.type === 'academic' || n.category === 'Academic' || n.type === 'resource');
            } else if (currentTabFilter === 'system') {
                filtered = items.filter(n => n.type === 'system' || n.category === 'System' || n.type === 'alert' || n.type === 'moderation');
            }

            if (filtered.length === 0) {
                listBody.innerHTML = `
                    <div class="notification-empty-dropdown">
                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
                            <path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"></path>
                            <path d="M13.73 21a2 2 0 0 1-3.46 0"></path>
                        </svg>
                        <h4>No notifications</h4>
                        <p>${currentTabFilter === 'unread' ? 'You have read all your alerts!' : 'No alerts in this category.'}</p>
                    </div>
                `;
                return;
            }

            listBody.innerHTML = filtered.map(item => `
                <div class="notification-item ${item.isRead ? 'is-read' : 'is-unread'}" 
                     data-notif-id="${item.id}"
                     data-notif-link="${item.link || ''}"
                     role="button"
                     tabindex="0"
                     aria-label="${item.title}">
                    <div class="notification-item-icon ${getCategoryIconClass(item.type)}">
                        <span>${item.icon || '🔔'}</span>
                    </div>
                    <div class="notification-item-content">
                        <div class="notification-item-header">
                            <h4 class="notification-item-title">${item.title}</h4>
                            <span class="notification-item-time">${item.timestamp}</span>
                        </div>
                        <p class="notification-item-message">${item.message}</p>
                        <div class="notification-item-footer">
                            <span class="notification-item-tag">${item.category || 'General'}</span>
                            ${item.link ? `<span class="notification-item-link-text">${item.actionLabel || 'View'} &rarr;</span>` : ''}
                        </div>
                    </div>
                    <span class="notification-item-dot" title="Unread"></span>
                </div>
            `).join('');
        }

        // Toggle Open/Close
        function openDropdown() {
            dropdown.classList.add('is-open');
            bellBtn.setAttribute('aria-expanded', 'true');
            renderDropdownList();
            updateBadge();
        }

        function closeDropdown() {
            dropdown.classList.remove('is-open');
            bellBtn.setAttribute('aria-expanded', 'false');
        }

        bellBtn.addEventListener('click', function (e) {
            e.stopPropagation();
            if (dropdown.classList.contains('is-open')) {
                closeDropdown();
            } else {
                openDropdown();
            }
        });

        // Close on outside click
        document.addEventListener('click', function (e) {
            if (!dropdown.contains(e.target) && !bellBtn.contains(e.target)) {
                closeDropdown();
            }
        });

        // Close on ESC
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape' && dropdown.classList.contains('is-open')) {
                closeDropdown();
            }
        });

        // Tab filter clicks
        tabBtns.forEach(btn => {
            btn.addEventListener('click', function () {
                tabBtns.forEach(b => b.classList.remove('is-active'));
                this.classList.add('is-active');
                currentTabFilter = this.getAttribute('data-filter') || 'all';
                renderDropdownList();
            });
        });

        // Item Click Handler (Mark as read & optional navigation)
        listBody.addEventListener('click', function (e) {
            const itemEl = e.target.closest('.notification-item');
            if (!itemEl) return;
            const id = itemEl.getAttribute('data-notif-id');
            const link = itemEl.getAttribute('data-notif-link');

            service.markAsRead(id);

            if (link) {
                window.location.href = link;
            }
        });

        // Mark All As Read
        if (markAllBtn) {
            markAllBtn.addEventListener('click', function () {
                service.markAllAsRead();
                StudyHub.showToast('All notifications marked as read', 'success');
            });
        }

        // Clear Read
        if (clearReadBtn) {
            clearReadBtn.addEventListener('click', function () {
                service.clearAllRead();
                StudyHub.showToast('Cleared all read notifications', 'success');
            });
        }

        // Event listener for state changes
        document.addEventListener('studyhub:notifications-updated', function () {
            updateBadge();
            if (dropdown.classList.contains('is-open')) {
                renderDropdownList();
            }
        });

        // Initial setup
        updateBadge();
    }

    // --- 4. Dedicated Notification Center Page Controller ---
    function initNotificationCenterPage() {
        const pageEl = document.querySelector('.notif-center-page');
        if (!pageEl) return;

        const statTotal = document.getElementById('statTotalNotifs');
        const statUnread = document.getElementById('statUnreadNotifs');
        const statRead = document.getElementById('statReadNotifs');
        const statAlerts = document.getElementById('statAlertsNotifs');

        const roleSelect = document.getElementById('notifRoleSelect');
        const searchInput = document.getElementById('notifSearchInput');
        const statusTabs = document.querySelectorAll('[data-status-filter]');
        const notifList = document.getElementById('notifPageList');
        const emptyState = document.getElementById('notifPageEmpty');

        const markAllBtn = document.getElementById('btnPageMarkAllRead');
        const clearReadBtn = document.getElementById('btnPageClearRead');
        const resetBtn = document.getElementById('btnPageResetDefaults');

        let currentStatusFilter = 'all';
        let searchQuery = '';

        // Initialize Role Select
        if (roleSelect) {
            roleSelect.value = service.getActiveRole();
            roleSelect.addEventListener('change', function () {
                service.setActiveRole(this.value);
                StudyHub.showToast(`Switched view to ${this.value} notifications`, 'success');
            });
        }

        function updateStats() {
            const items = service.getAll();
            const unread = items.filter(n => !n.isRead).length;
            const read = items.length - unread;
            const alerts = items.filter(n => n.type === 'alert' || n.category === 'Alert').length;

            if (statTotal) statTotal.textContent = items.length;
            if (statUnread) statUnread.textContent = unread;
            if (statRead) statRead.textContent = read;
            if (statAlerts) statAlerts.textContent = alerts;
        }

        function renderPageList() {
            if (!notifList) return;
            const items = service.getAll();

            let filtered = items.filter(item => {
                // Status filter
                if (currentStatusFilter === 'unread' && item.isRead) return false;
                if (currentStatusFilter === 'read' && !item.isRead) return false;

                // Search query
                if (searchQuery) {
                    const text = (item.title + ' ' + item.message + ' ' + (item.category || '')).toLowerCase();
                    if (!text.includes(searchQuery)) return false;
                }
                return true;
            });

            updateStats();

            if (filtered.length === 0) {
                notifList.style.display = 'none';
                if (emptyState) emptyState.style.display = 'block';
                return;
            }

            notifList.style.display = 'flex';
            if (emptyState) emptyState.style.display = 'none';

            notifList.innerHTML = filtered.map(item => `
                <article class="notif-page-card ${item.isRead ? 'is-read' : 'is-unread'}" data-notif-id="${item.id}">
                    <div class="notif-page-card-icon ${getCategoryIconClass(item.type)}">
                        <span>${item.icon || '🔔'}</span>
                    </div>
                    <div class="notif-page-card-body">
                        <div class="notif-page-card-top">
                            <h3 class="notif-page-card-title">${item.title}</h3>
                            <div class="notif-page-card-badges">
                                <span class="badge ${item.isRead ? 'badge-outline' : 'badge-primary'}">${item.isRead ? 'Read' : '● Unread'}</span>
                                <span class="badge badge-outline">${item.category || 'General'}</span>
                            </div>
                        </div>
                        <p class="notif-page-card-message">${item.message}</p>
                        <div class="notif-page-card-bottom">
                            <div class="notif-page-card-time">
                                <span>🕒</span>
                                <span>${item.timestamp}</span>
                                <span style="opacity:0.4;">•</span>
                                <span>Target: <strong>${item.role || 'All'}</strong></span>
                            </div>
                            <div class="notif-page-card-btn-group">
                                ${item.link ? `<a class="notif-action-link-btn" href="${item.link}">${item.actionLabel || 'View Action'} &rarr;</a>` : ''}
                                <button class="notif-toggle-read-btn" type="button" data-action="toggle-read" data-id="${item.id}">
                                    ${item.isRead ? 'Mark as unread' : '✓ Mark read'}
                                </button>
                                <button class="notif-delete-btn" type="button" data-action="delete" data-id="${item.id}" title="Delete notification">
                                    🗑️
                                </button>
                            </div>
                        </div>
                    </div>
                </article>
            `).join('');
        }

        // Status Tabs Click
        statusTabs.forEach(tab => {
            tab.addEventListener('click', function () {
                statusTabs.forEach(t => t.classList.remove('is-active'));
                this.classList.add('is-active');
                currentStatusFilter = this.getAttribute('data-status-filter') || 'all';
                renderPageList();
            });
        });

        // Search Input
        if (searchInput) {
            searchInput.addEventListener('input', function () {
                searchQuery = this.value.trim().toLowerCase();
                renderPageList();
            });
        }

        // Inline Actions (Toggle read / Delete)
        notifList.addEventListener('click', function (e) {
            const btn = e.target.closest('button[data-action]');
            if (!btn) return;
            const action = btn.getAttribute('data-action');
            const id = btn.getAttribute('data-id');

            if (action === 'toggle-read') {
                const item = service.getAll().find(n => n.id === id);
                if (item) {
                    if (item.isRead) {
                        service.markAsUnread(id);
                        StudyHub.showToast('Marked as unread');
                    } else {
                        service.markAsRead(id);
                        StudyHub.showToast('Marked as read', 'success');
                    }
                }
            } else if (action === 'delete') {
                service.deleteNotification(id);
                StudyHub.showToast('Notification removed', 'warning');
            }
        });

        // Bulk Actions
        if (markAllBtn) {
            markAllBtn.addEventListener('click', function () {
                service.markAllAsRead();
                StudyHub.showToast('All notifications marked as read', 'success');
            });
        }

        if (clearReadBtn) {
            clearReadBtn.addEventListener('click', function () {
                service.clearAllRead();
                StudyHub.showToast('All read notifications cleared', 'success');
            });
        }

        if (resetBtn) {
            resetBtn.addEventListener('click', function () {
                service.resetDefaults();
                StudyHub.showToast('Demo notifications restored to defaults', 'success');
            });
        }

        // Sync on updates
        document.addEventListener('studyhub:notifications-updated', function () {
            if (roleSelect) roleSelect.value = service.getActiveRole();
            renderPageList();
        });

        // Initial render
        renderPageList();
    }

    // --- 5. Boot Engine ---
    document.addEventListener('DOMContentLoaded', function () {
        initNavbarDropdown();
        initNotificationCenterPage();
    });

    if (document.readyState === 'interactive' || document.readyState === 'complete') {
        initNavbarDropdown();
        initNotificationCenterPage();
    }

})();
