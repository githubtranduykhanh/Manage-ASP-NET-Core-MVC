class Loading {
    constructor({ color }) {
        // Tạo spinner
        this.spinner = document.createElement('div');
        this.spinner.className = `spinner-border spinner-border-lg ${color}`;
        this.spinner.style.width = '5rem';
        this.spinner.style.height = '5rem';
        this.spinner.setAttribute('role', 'status');
        this.spinner.innerHTML = '<span class="visually-hidden">Loading...</span>';

        // Tạo modal
        this.modal = document.createElement('div');
        this.modal.className = 'modal fade';
        this.modal.style.zIndex = '99999';
        
        this.modal.setAttribute('tabindex', '-1');
        this.modal.setAttribute('aria-hidden', 'true');

        const modalDialog = document.createElement('div');
        modalDialog.className = 'modal-dialog modal-dialog-centered modal-sm';
        modalDialog.style.justifyContent = 'center';      

        modalDialog.appendChild(this.spinner);
        this.modal.appendChild(modalDialog);

        document.body.appendChild(this.modal);

        this.bootstrapModal = new bootstrap.Modal(this.modal, {
            backdrop: 'static',
            keyboard: false
        });

       
    }

    async show() {    
        this.bootstrapModal.show();
        return await this.modalShown()       
    }

    hide() {
        this.bootstrapModal.hide();
    }

    dispose() {
        this.bootstrapModal.hide();
        document.body.removeChild(this.modal);
    }

    modalShown() {
        return new Promise((resolve) => {
            $(this.modal).one('shown.bs.modal', () => {
                resolve();
            });
        });
    }

    isModalShown() {
        return this.bootstrapModal._isShown;
    }
}

export default Loading
