class Modal {
    constructor() {}

    createModal({ id, title, size = 'xl' }) {
        // Create modal elements
        const modal = document.createElement('div');
        modal.classList.add('modal', 'fade', 'show');
        modal.id = id;
        modal.tabIndex = -1;
        modal.setAttribute('aria-modal', 'true');
        modal.setAttribute('role', 'dialog');      

        const dialog = document.createElement('div');
        dialog.classList.add('modal-dialog', `modal-${size}`);
        dialog.setAttribute('role', 'document');

        const content = document.createElement('div');
        content.classList.add('modal-content');

        const header = document.createElement('div');
        header.classList.add('modal-header');

        const titleElement = document.createElement('h5');
        titleElement.classList.add('modal-title');
        titleElement.innerText = title;

        const closeButton = document.createElement('button');
        closeButton.type = 'button';
        closeButton.classList.add('btn-close');
        closeButton.setAttribute('data-bs-dismiss', 'modal');
        closeButton.setAttribute('aria-label', 'Close');

        const body = document.createElement('div');
        body.classList.add('modal-body');

        // Append elements
        header.appendChild(titleElement);
        header.appendChild(closeButton);

        content.appendChild(header);
        content.appendChild(body);

        dialog.appendChild(content);
        modal.appendChild(dialog);

        document.body.appendChild(modal);

        return { modalElement: modal, bodyElement: body, titleElement: titleElement };
    }

    updateTitle(modal, newTitle) {
        const titleElement = modal.querySelector('.modal-title');
        if (titleElement) {
            titleElement.innerText = newTitle;
        }
    }

    addContentToBody(modal, content) {
        const body = modal.querySelector('.modal-body');
        if (body) {
            body.innerHTML = content;
        }
    }

    removeModal(modal) {
        if (modal && modal.parentNode) {
            modal.parentNode.removeChild(modal);
        }
    }
}