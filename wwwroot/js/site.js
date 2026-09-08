/**
 * StudyHub - Comprehensive Client Interactivity System
 * Theme Management, Live Filtering, Preview Modals, Toasts & Focus Tracking
 */

(function () {
    'use strict';

    // --- 1. Global StudyHub Namespace & Utilities ---
    window.StudyHub = window.StudyHub || {};

    // Toast Notification Manager
    StudyHub.showToast = function (message, type = 'success') {
        let container = document.querySelector('.toast-container');
        if (!container) {
            container = document.createElement('div');
            container.className = 'toast-container';
            container.setAttribute('aria-live', 'polite');
            document.body.appendChild(container);
        }

        const toast = document.createElement('div');
        toast.className = 'toast-alert';
        toast.setAttribute('role', 'status');

        const icon = type === 'success' ? '✓' : type === 'warning' ? '★' : 'ℹ';
        toast.innerHTML = `
            <span class="toast-icon">${icon}</span>
            <span class="toast-message">${message}</span>
        `;

        container.appendChild(toast);

        setTimeout(() => {
            toast.classList.add('is-hiding');
            setTimeout(() => toast.remove(), 250);
        }, 3200);
    };

    // --- 2. Theme Switcher Engine (Light / Dark Mode) ---
    const root = document.documentElement;
    const themeToggles = document.querySelectorAll('[data-theme-toggle]');
    const savedTheme = localStorage.getItem('studyhub-theme');

    function applyTheme(theme) {
        root.dataset.theme = theme;
        themeToggles.forEach(toggle => {
            const isDark = theme === 'dark';
            toggle.setAttribute('aria-label', isDark ? 'Switch to light mode' : 'Switch to dark mode');
            toggle.setAttribute('title', isDark ? 'Switch to light mode' : 'Switch to dark mode');
        });
    }

    applyTheme(savedTheme === 'dark' ? 'dark' : 'light');

    themeToggles.forEach(toggle => {
        toggle.addEventListener('click', function () {
            const nextTheme = root.dataset.theme === 'dark' ? 'light' : 'dark';
            localStorage.setItem('studyhub-theme', nextTheme);
            applyTheme(nextTheme);
            StudyHub.showToast(nextTheme === 'dark' ? 'Dark mode enabled' : 'Light mode enabled');
        });
    });

    // --- 3. Resource Preview Modal Engine ---
    const modalOverlay = document.getElementById('resourcePreviewModal');
    const modalTitle = document.getElementById('modalResourceTitle');
    const modalBadge = document.getElementById('modalResourceBadge');
    const modalCode = document.getElementById('modalResourceCode');
    const modalMeta = document.getElementById('modalResourceMeta');
    const modalCloseBtns = document.querySelectorAll('[data-modal-close]');

    function openModal(data) {
        if (!modalOverlay) return;
        if (modalTitle) modalTitle.textContent = data.title || 'Resource Details';
        if (modalBadge) modalBadge.textContent = data.badge || 'Academic';
        if (modalCode) modalCode.textContent = data.code || '';
        if (modalMeta) modalMeta.textContent = data.meta || 'Available for download and preview.';
        modalOverlay.classList.add('is-open');
        document.body.style.overflow = 'hidden';
    }

    function closeModal() {
        if (!modalOverlay) return;
        modalOverlay.classList.remove('is-open');
        document.body.style.overflow = '';
    }

    modalCloseBtns.forEach(btn => btn.addEventListener('click', closeModal));
    if (modalOverlay) {
        modalOverlay.addEventListener('click', function (e) {
            if (e.target === modalOverlay) closeModal();
        });
    }

    window.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && modalOverlay && modalOverlay.classList.contains('is-open')) {
            closeModal();
        }
    });

    // Delegate Preview Button Clicks
    document.addEventListener('click', function (e) {
        const previewBtn = e.target.closest('[data-preview-trigger]');
        if (previewBtn) {
            e.preventDefault();
            const card = previewBtn.closest('.document-card, .lecture-card, .study-resource-grid a');
            if (card) {
                const title = card.querySelector('h2, strong')?.textContent?.trim();
                const badge = card.querySelector('.badge, .subject-tag')?.textContent?.trim();
                const code = card.querySelector('.document-course-code, p, small')?.textContent?.trim();
                const meta = card.querySelector('.document-meta-row, .card-body small, .lecture-footer')?.textContent?.trim();
                openModal({ title, badge, code, meta });
            }
        }
    });

    // --- 4. Live Client-Side Catalog Search & Filtering ---
    function applyCatalogFilters() {
        const catalogContainer = document.querySelector('.catalog-layout');
        if (!catalogContainer) return;

        const items = catalogContainer.querySelectorAll('.document-card, .lecture-card');
        const checkedCheckboxes = Array.from(catalogContainer.querySelectorAll('.filter-panel input[type="checkbox"]:checked'));
        const checkedTexts = checkedCheckboxes.map(cb => cb.parentElement.textContent.trim().toLowerCase());

        let visibleCount = 0;

        items.forEach(item => {
            const itemText = item.textContent.toLowerCase();
            let isVisible = true;

            if (checkedTexts.length > 0) {
                isVisible = checkedTexts.some(filterTerm => itemText.includes(filterTerm));
            }

            item.style.display = isVisible ? '' : 'none';
            if (isVisible) visibleCount++;
        });

        const countBadge = document.querySelector('.catalog-stats-pill strong');
        if (countBadge) {
            countBadge.textContent = `${visibleCount} item${visibleCount === 1 ? '' : 's'}`;
        }
    }

    document.querySelectorAll('.filter-panel input[type="checkbox"]').forEach(checkbox => {
        checkbox.addEventListener('change', applyCatalogFilters);
    });

    document.querySelector('.filter-clear-btn')?.addEventListener('click', function () {
        document.querySelectorAll('.filter-panel input[type="checkbox"]').forEach(cb => {
            cb.checked = false;
        });
        applyCatalogFilters();
        StudyHub.showToast('All filters cleared');
    });

    // Global Header Search Interactivity
    const headerSearchInput = document.querySelector('.search-box input');
    if (headerSearchInput) {
        headerSearchInput.addEventListener('input', function () {
            const query = this.value.trim().toLowerCase();
            const catalogItems = document.querySelectorAll('.document-card, .lecture-card, .study-resource-grid a');
            if (catalogItems.length > 0) {
                catalogItems.forEach(item => {
                    const text = item.textContent.toLowerCase();
                    item.style.display = text.includes(query) ? '' : 'none';
                });
            }
        });
    }

    // --- 5. Interactive Favorite Star Buttons ---
    document.addEventListener('click', function (e) {
        const starBtn = e.target.closest('.star-btn:not(.favorite-remove)');
        if (starBtn) {
            e.preventDefault();
            starBtn.classList.toggle('is-active');
            const isActive = starBtn.classList.contains('is-active');
            starBtn.textContent = isActive ? '★' : '☆';
            StudyHub.showToast(isActive ? 'Saved to Favorites' : 'Removed from Favorites', isActive ? 'success' : 'warning');
        }
    });

    // --- 6. Favorites Page Removal & Undo ---
    document.querySelectorAll('.favorite-remove').forEach(button => {
        button.addEventListener('click', function () {
            const card = this.closest('.document-card');
            if (card) {
                card.style.transition = 'all 0.25s ease';
                card.style.opacity = '0';
                card.style.transform = 'translateX(20px)';
                setTimeout(() => {
                    card.remove();
                    const remaining = document.querySelectorAll('.favorites-list .document-card');
                    const countEl = document.querySelector('.favorites-count-number');
                    if (countEl) countEl.textContent = remaining.length;
                    if (remaining.length === 0) {
                        const emptyState = document.querySelector('.favorites-empty-state');
                        if (emptyState) emptyState.hidden = false;
                    }
                    StudyHub.showToast('Resource removed from Favorites', 'warning');
                }, 250);
            }
        });
    });

    document.querySelector('[data-clear-favorites]')?.addEventListener('click', function () {
        const cards = document.querySelectorAll('.favorites-list .document-card');
        cards.forEach(card => {
            card.style.transition = 'all 0.25s ease';
            card.style.opacity = '0';
        });
        setTimeout(() => {
            cards.forEach(c => c.remove());
            const countEl = document.querySelector('.favorites-count-number');
            if (countEl) countEl.textContent = '0';
            const emptyState = document.querySelector('.favorites-empty-state');
            if (emptyState) emptyState.hidden = false;
            StudyHub.showToast('All favorites cleared');
        }, 250);
    });

    // --- 7. Dashboard Focus Action Toggle ---
    document.querySelector('[data-focus-action]')?.addEventListener('click', function () {
        this.classList.toggle('is-added');
        const isAdded = this.classList.contains('is-added');
        this.innerHTML = isAdded ? '✓ Focus active' : '+ Add focus';
        StudyHub.showToast(isAdded ? 'New focus goal activated for today!' : 'Focus goal updated');
    });

    // --- 8. Mobile Navigation Menu Toggle ---
    const mobileMenuBtn = document.querySelector('.mobile-menu-btn');
    const resourceNav = document.querySelector('.resource-nav');
    if (mobileMenuBtn && resourceNav) {
        mobileMenuBtn.addEventListener('click', function () {
            resourceNav.classList.toggle('is-mobile-open');
        });
    }

})();

(function () {
    const trigger = document.querySelector('[data-profile-lightbox]');
    const panel = document.querySelector('[data-profile-lightbox-panel]');
    if (!trigger || !panel) return;

    const close = () => {
        panel.classList.remove('is-open');
        panel.setAttribute('aria-hidden', 'true');
    };

    trigger.addEventListener('click', () => {
        panel.classList.add('is-open');
        panel.setAttribute('aria-hidden', 'false');
    });
    panel.querySelectorAll('[data-profile-lightbox-close]').forEach(element => element.addEventListener('click', close));
    document.addEventListener('keydown', event => {
        if (event.key === 'Escape') close();
    });
})();
