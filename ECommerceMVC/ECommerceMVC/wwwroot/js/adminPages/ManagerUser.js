import { apiGetUserById, apiEditUser } from "../appApi/user.js";
import { apiGetRoles } from "../appApi/role.js";
import { ConsoleErrorCatch, ConsoleErrorStatus } from "../helper/HelperError.js";
import { ToastBody } from "../helper/HelperToasts.js";
import { formatDatatime } from "../helper/HelperString.js";
import Loading from "../helper/HelperLoading.js";
import NProgress from 'https://cdn.jsdelivr.net/npm/nprogress@0.2.0/+esm';

// Tạo class quản lý người dùng
class ManagerUser {
    constructor() {
        this.tableUsers = null;
        this.toast = null;
        this.modelEdit = null;
        this.modelConfirmationDelete = null;
        this.offCanvasEl = null;
        this.inputBirthday = null;
        this.loading = null;
        this.modalEditElement = document.querySelector("#exLargeModalEdit");
        this.modalConfirmationDeleteElement = document.querySelector("#confirmationDelete");
        this.btnTableEdits = document.querySelectorAll(".js-action-edit");
        this.btnTableDeletes = document.querySelectorAll(".js-action-delete");
        this.btnModalEdit = document.querySelector("#js-btn-model-edit");
        this.upload = document.querySelector("#upload");
        this.uploadedAvatar = document.querySelector("#uploadedAvatar");
        this.displayExLarge = document.querySelector("#displayExLarge");
        this.genderExLarge = document.querySelector("#genderExLarge");
        this.birthdayExLarge = document.querySelector("#birthdayExLarge");
        this.inputIdUserDelete = this.modalConfirmationDeleteElement.querySelector("#inputUserIdDelete");
        this.accordionRoles = document.querySelector("#accordion-roles");
        this.accordionRolesBody = this.accordionRoles.querySelector(".accordion-body");
    }

    init() {
        this.loadingConfig();
        this.modalInit();
        this.toast = new ToastBody({
            placement: 'top-0 end-0',
            title: 'E Commerce',
            icon: '<i class="bx bx-bell me-2"></i>',
            toastOptions: { delay: 2000, autohide: true }
        });
        this.tableInit();
        document.querySelector('div.head-label').innerHTML = '<h5 class="card-title mb-0">DataTable User</h5>';
        setTimeout(() => {
            this.resetCreateNew();
        }, 200);
        this.inputDataTimeInit();
        this.callRolesEdit();
        this.addEvents();
    }

    inputDataTimeInit() {
        const dataTimeInputs = document.querySelectorAll(".dt-date");
        Array.from(dataTimeInputs).forEach(input => {
            flatpickr(input, {
                enableTime: true,
                dateFormat: "Y-m-d H:i",
                time_24hr: false,
                altInput: true,
                altFormat: "F j, Y",
            });
        });

        this.inputBirthday = flatpickr(this.birthdayExLarge, {
            enableTime: true,
            dateFormat: "Z",
            altInput: true,
            altFormat: "F j, Y h:i K",
            time_24hr: false
        });
    }

    modalInit() {
        this.modelEdit = new bootstrap.Modal(this.modalEditElement);
        this.modelConfirmationDelete = new bootstrap.Modal(this.modalConfirmationDeleteElement);
    }

    tableInit() {
        this.tableUsers = new DataTable('#js-table-users', {
            responsive: true,
            dom: '<"card-header flex-column flex-md-row"<"head-label text-center"><"dt-action-buttons text-end pt-3 pt-md-0"B>><"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6 d-flex justify-content-center justify-content-md-end"f>>t<"row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
            buttons: [
                {
                    extend: "collection",
                    className: "btn btn-label-primary dropdown-toggle me-2",
                    text: '<i class="bx bx-export me-sm-1"></i> <span class="d-none d-sm-inline-block">Export</span>',
                    buttons: [
                        this.exportButtonConfig("print", "bx bx-printer me-1", "Print"),
                        this.exportButtonConfig("csv", "bx bx-file me-1", "Csv"),
                        this.exportButtonConfig("excel", "bx bxs-file-export me-1", "Excel"),
                        this.exportButtonConfig("pdf", "bx bxs-file-pdf me-1", "Pdf"),
                        this.exportButtonConfig("copy", "bx bx-copy me-1", "Copy")
                    ]
                },
                {
                    text: '<i class="bx bx-plus me-sm-1"></i> <span class="d-none d-sm-inline-block">Add New Record</span>',
                    className: "create-new btn btn-primary"
                }
            ],
            columnDefs: [
                { targets: 4, render: data => data.length > 25 ? data.substr(0, 25) + '...' : data },
                { targets: 2, render: data => data.length > 20 ? data.substr(0, 20) + '...' : data }
            ]
        });
    }

    exportButtonConfig(type, iconClass, text) {
        return {
            extend: type,
            text: `<i class="${iconClass}"></i>${text}`,
            className: "dropdown-item",
            title: 'User List',
            filename: () => `User List - ${Math.floor(Date.now() / 1000)}`,
            exportOptions: {
                columns: [1, 2, 3, 4, 5, 6, 7, 8, 9],
                format: {
                    body: (data, row, column, node) => (column === 3 || column === 1) ? ($(node).attr('data-original-value') || data) : ($(node).find(':input').val() || data)
                }
            }
        };
    }

    loadingConfig() {
        NProgress.configure({
            minimum: 0.1,
            easing: 'ease',
            speed: 500,
            trickle: true,
            trickleSpeed: 200,
            showSpinner: true,
            parent: 'body'
        });

        this.loading = new Loading({
            lines: 13,
            length: 38,
            width: 17,
            radius: 45,
            scale: 0.65,
            corners: 1,
            speed: 1,
            rotate: 0,
            animation: 'spinner-line-fade-quick',
            direction: 1,
            color: '#5f61e6',
            fadeColor: 'white',
            top: '50%',
            left: '50%',
            shadow: '0 0 1px transparent',
            zIndex: 2000000000,
            className: 'spinner',
            position: 'absolute'
        });
    }

    async callRolesEdit() {
        try {
            const res = await apiGetRoles();
            if (res?.status) {
                this.accordionRolesBody.innerHTML = "";
                res?.data?.forEach(role => {
                    const checkbox = document.createElement('input');
                    checkbox.type = 'checkbox';
                    checkbox.name = 'roles';
                    checkbox.className = 'form-check-input';
                    checkbox.value = role.name;
                    checkbox.dataset.name = role.name;
                    checkbox.id = `role_${role.id}`;

                    const label = document.createElement('label');
                    label.className = "form-label";
                    label.htmlFor = `role_${role.id}`;
                    label.appendChild(document.createTextNode(role.name));

                    this.accordionRolesBody.appendChild(checkbox);
                    this.accordionRolesBody.appendChild(label);
                    this.accordionRolesBody.appendChild(document.createElement('br'));
                });
            }
        } catch (error) {
            ConsoleErrorCatch(error);
        }
    }

    getCheckedRoles() {
        const checkboxes = document.querySelectorAll('input[type="checkbox"][name="roles"]');
        return Array.from(checkboxes).filter(checkbox => checkbox.checked).map(checkbox => checkbox.dataset.name);
    }

    resetCreateNew() {
        const createNewBtn = document.querySelector(".create-new");
        const newRecordCanvas = document.querySelector("#add-new-record");
        if (createNewBtn) {
            createNewBtn.addEventListener("click", () => {
                this.offCanvasEl = new bootstrap.Offcanvas(newRecordCanvas);
                newRecordCanvas.querySelector(".dt-full-name").value = "";
                newRecordCanvas.querySelector(".dt-post").value = "";
                newRecordCanvas.querySelector(".dt-email").value = "";
                newRecordCanvas.querySelector(".dt-date").value = "";
                newRecordCanvas.querySelector(".dt-salary").value = "";
                this.offCanvasEl.show();
            });
        }
    }

    addEvents() {
        this.btnTableDeletes.forEach(btn => {
            btn.addEventListener("click", (e) => {
                this.inputIdUserDelete.value = e.currentTarget.dataset.id;
                this.modelConfirmationDelete.show();
            });
        });
        this.btnTableEdits.forEach(btn => {
            btn.addEventListener("click", async (e) => {
                const btnElement = e.currentTarget;
                try {
                    const res = await apiGetUserById(btnElement.dataset.id);
                    if (res?.status) {
                        this.modalEditElement.querySelector("#inputId").value = res.data.id;
                        this.modalEditElement.querySelector("#inputUserName").value = res.data.userName;
                        this.modalEditElement.querySelector("#inputEmail").value = res.data.email;
                        this.modalEditElement.querySelector("#inputPhoneNumber").value = res.data.phoneNumber;
                        this.modalEditElement.querySelector("#inputFirstName").value = res.data.firstName;
                        this.modalEditElement.querySelector("#inputLastName").value = res.data.lastName;
                        this.inputBirthday.setDate(res.data.birthday, true);
                        this.uploadedAvatar.src = res.data.avatar;
                        this.callRolesEdit();
                        this.modelEdit.show();
                    }
                } catch (error) {
                    ConsoleErrorCatch(error);
                }
            });
        });
        this.btnModalEdit.addEventListener("click", async (e) => {
            const formData = new FormData(this.modalEditElement.querySelector("form"));
            formData.append('roles', JSON.stringify(this.getCheckedRoles()));
            NProgress.start();
            this.loading.show();
            try {
                const res = await apiEditUser(formData);
                if (res?.status) {
                    this.toast.setMessage(res.message);
                    this.toast.showToast();
                    this.modelEdit.hide();
                    this.tableUsers.ajax.reload();
                }
            } catch (error) {
                ConsoleErrorCatch(error);
            }
            NProgress.done();
            this.loading.hide();
        });
    }
}


export default ManagerUser


