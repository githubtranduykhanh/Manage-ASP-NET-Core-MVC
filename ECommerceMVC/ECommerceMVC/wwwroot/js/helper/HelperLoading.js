import { Spinner } from 'https://cdn.skypack.dev/spin.js'


class Loading {
    constructor(spinnerOpts) {
        // Tạo spinner
        this.spinner = this.createSpinnerElement()
        // Tạo modal
        this.modal = this.createModalElement(this.spinner)
        document.body.appendChild(this.modal);

        this.bootstrapModal = new bootstrap.Modal(this.modal, {
            backdrop: 'static',
            keyboard: false
        });

        this.spinnerInstance = new Spinner(spinnerOpts)
    }


    createSpinnerElement() {
        // Tạo spinner
        const spinner = document.createElement('div');
        //this.spinner.className = `spinner-border spinner-border-lg ${color}`;
        spinner.style.width = '5rem';
        spinner.style.height = '5rem';
        spinner.style.margin = '50px auto';
        spinner.style.position = 'relative';

        //this.spinner.setAttribute('role', 'status');
        //this.spinner.innerHTML = '<span class="visually-hidden">Loading...</span>';

        spinner.id = 'spinner';
        return spinner
    }

    createModalElement(spinner) {
        // Tạo modal
        const modal = document.createElement('div');
        modal.className = 'modal fade';
        modal.style.zIndex = '99999';

        modal.setAttribute('tabindex', '-1');
        modal.setAttribute('aria-hidden', 'true');

        const modalDialog = document.createElement('div');
        modalDialog.className = 'modal-dialog modal-dialog-centered modal-sm';
        modalDialog.style.justifyContent = 'center';

        modalDialog.appendChild(spinner);
        modal.appendChild(modalDialog);

        return modal
    }

    async show() {  
        this.spinnerInstance.spin(this.spinner)
        this.bootstrapModal.show();  
        return await this.modalShown()       
    }

    hide() {
        this.spinnerInstance.stop();
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
