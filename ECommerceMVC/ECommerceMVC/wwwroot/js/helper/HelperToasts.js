
export class Toast {
    constructor({ selector, placement, toastOptions }) {
        this.toastPlacementExample = document.querySelector(selector);
        this.placement = placement;
        this.toastPlacement = null;
        this.toastOptions = toastOptions || {};
    }

    // Xóa toast cũ nếu tồn tại
    toastDispose() {
        if (this.toastPlacement && this.toastPlacement._element !== null) {
            if (this.toastPlacementExample) {
                DOMTokenList.prototype.remove.apply(this.toastPlacementExample.classList, this.placement.split(' '));
                this.toastPlacementExample.className = 'bs-toast toast toast-placement-ex m-2 fade'; // Xóa tất cả các class khác               
            }
            this.toastPlacement.dispose();
        }
    }

    // Hiển thị toast với type và message
    showToast(type, message) {
        this.toastDispose();

        // Cấu hình type và placement
        this.toastPlacementExample.classList.add(type);
        DOMTokenList.prototype.add.apply(this.toastPlacementExample.classList, this.placement.split(' '));

        // Cập nhật thông điệp
        this.toastPlacementExample.querySelector('.toast-body').innerText = message;

        // Tạo và hiển thị toast
        this.toastPlacement = new bootstrap.Toast(this.toastPlacementExample, this.toastOptions);
        this.toastPlacement.show();
    }

    success(message) {
        this.showToast('bg-success', message);
    }

    warning(message) {
        this.showToast('bg-warning', message);
    }

    danger(message) {
        this.showToast('bg-danger', message);
    }

    primary(message) {
        this.showToast('bg-primary', message);
    }

    secondary(message) {
        this.showToast('bg-secondary', message);
    }

    info(message) {
        this.showToast('bg-info', message);
    }

    dark(message) {
        this.showToast('bg-dark', message);
    }
}


export class ToastBody {
    constructor({ placement, icon, title, toastOptions }) {
        this.icon = icon;
        this.title = title;
        this.placement = placement;
        this.toastOptions = toastOptions || {};
        this.toastElement = this.createToastElement();
        document.body.appendChild(this.toastElement);
        this.toastPlacement = new bootstrap.Toast(this.toastElement, this.toastOptions);
    }

    // Tạo phần tử toast
    createToastElement() {
        const toastElement = document.createElement('div');     
        toastElement.className = 'bs-toast toast toast-placement-ex m-2 fade';
        toastElement.setAttribute('role', 'alert');
        toastElement.setAttribute('aria-live', 'assertive');
        toastElement.setAttribute('aria-atomic', 'true');
        toastElement.setAttribute('data-bs-delay', '2000');

        const toastHeader = document.createElement('div');
        toastHeader.className = 'toast-header';
        toastHeader.innerHTML = `
            ${this.icon}
            <div class="me-auto fw-medium">${this.title}</div>
            <small>${this.getFormattedTime()}</small>
            <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
        `;
        toastElement.appendChild(toastHeader);

        const toastBody = document.createElement('div');
        toastBody.className = 'toast-body';
        toastElement.appendChild(toastBody);

        // Thêm các lớp placement vào toastElement
        toastElement.classList.add(...this.placement.split(' '));

        return toastElement;
    }


    // Lấy thời gian định dạng
    getFormattedTime() {
        const delay = this.toastOptions.delay || 2000;
        const minutes = Math.floor(delay / 1000); // Chuyển đổi thành phút
        return `${minutes} secs ago`;
    }

    // Xóa toast
    toastDispose() {
        if (this.toastPlacement._element !== null) {
            this.toastPlacement.dispose();
            this.toastElement.remove();
        }
    }

    // Hiển thị toast với type và message
    showToast(type, message) {
        // Cập nhật type và message   
        this.toastElement.className = 'bs-toast toast toast-placement-ex m-2 fade'; // Xóa tất cả các class khác
        this.toastElement.classList.add(...this.placement.split(' '));
        this.toastElement.classList.add(type);
        this.toastElement.querySelector('.toast-body').innerText = message;
        // Hiển thị toast
        this.toastPlacement.show();
    }

    // Hiển thị toast success
    success(message) {
        this.showToast('bg-success', message);
    }

    // Hiển thị toast warning
    warning(message) {
        this.showToast('bg-warning', message);
    }

    // Hiển thị toast danger
    danger(message) {
        this.showToast('bg-danger', message);
    }

    // Hiển thị toast primary
    primary(message) {
        this.showToast('bg-primary', message);
    }

    // Hiển thị toast secondary
    secondary(message) {
        this.showToast('bg-secondary', message);
    }

    // Hiển thị toast info
    info(message) {
        this.showToast('bg-info', message);
    }

    // Hiển thị toast dark
    dark(message) {
        this.showToast('bg-dark', message);
    }
}



