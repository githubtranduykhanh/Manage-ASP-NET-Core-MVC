import { apiGetUserById, apiEditUser, apiCreateClaimUser, apiGetClaimUser, apiEditClaimUser, apiDeleteClaimUser } from "../appApi/user.js"
import { apiGetRoles } from "../appApi/role.js"

import { ConsoleErrorCatch, ConsoleErrorStatus } from "../helper/HelperError.js"
import { ToastBody } from "../helper/HelperToasts.js"

import { formatDatatime } from "../helper/HelperString.js"


import Loading from "../helper/HelperLoading.js"


import NProgress from 'https://cdn.jsdelivr.net/npm/nprogress@0.2.0/+esm'

const managerUser = (() => {
    let tableUsers, toast, modelEdit, modelConfirmationDelete, offCanvasEl, inputBirthday, loading, tableClaim, modelRoleClaim, modalConfirmationDeleteRoleClaim, IdUserLogin
    const modalEditElement = document.querySelector("#exLargeModalEdit")
    const modalConfirmationDeleteElement = document.querySelector("#confirmationDelete")
    const btnTableEdits = document.querySelectorAll(".js-action-edit")
    const btnTableDeletes = document.querySelectorAll(".js-action-delete")
    const btnModalEdit = document.querySelector("#js-btn-model-edit")
    const upload = document.querySelector("#upload")
    const uploadedAvatar = document.querySelector("#uploadedAvatar")
    const displayExLarge = document.querySelector("#displayExLarge")
    const genderExLarge = document.querySelector("#genderExLarge")
    const birthdayExLarge = document.querySelector("#birthdayExLarge")
    const inputIdUserDelete = modalConfirmationDeleteElement.querySelector("#inputUserIdDelete")
    const accordionRoles = document.querySelector("#accordion-roles")
    const accordionRolesBody = accordionRoles.querySelector(".accordion-body")
    const tableClaimElement = document.querySelector("#js-table-role-claim")
    const modelRoleClaimElement = document.querySelector("#exLargeModalRoleClaim")
    const inputRoleType = modelRoleClaimElement.querySelector("#roleType")
    const inputRoleValue = modelRoleClaimElement.querySelector("#roleValue")
    let btnTableRoleClaimEdits = document.querySelectorAll(".js-action-claim-edit")
    let btnTableRoleClaimDeletes = document.querySelectorAll(".js-action-claim-delete")
    const btnModalRoleClaim = document.querySelector("#js-btn-model-role-claim")
    const titleModalRoleClaim = modelRoleClaimElement.querySelector("#js-title-role-claim")
    const modalConfirmationDeleteRoleClaimElement = document.querySelector("#confirmationDeleteRoleClaim")
    const btnConfirmationDeleteRoleClaim = document.querySelector("#js-btn-confirmation-delete-role-claim")
    const titleConfirmationDeleteRoleClaim = modalConfirmationDeleteRoleClaimElement.querySelector("#confirmationDeleteRoleClaimTitle")

    const managerUserStore = {
        init() {


            this.loadingConfig()

            this.modalInit()
          
            toast = new ToastBody({ placement: 'top-0 end-0', title: 'E Commerce', icon: '<i class="bx bx-bell me-2"></i>', toastOptions: { delay: 2000, autohide: true } })

            this.tableInit()

            const headLabel = document.querySelector('div.head-label')
            headLabel.innerHTML = '<h5 class="card-title mb-0">DataTable User</h5>'
            setTimeout(() => {
                this.resetCreateNew()
            }, 200)
            this.inputDataTimeInit()
            this.callRolesEdit()
            this.addEvents()
        },
        inputDataTimeInit() {
            const dataTimeInputs = document.querySelectorAll(".dt-date")
            Array.from(dataTimeInputs).forEach(input => {
                flatpickr(input, {
                    enableTime: true,
                    dateFormat: "Y-m-d H:i",
                    time_24hr: false, // Sử dụng định dạng 12 giờ (AM/PM), đổi thành true nếu muốn 24 giờ
                    /*monthSelectorType: "static",*/               
                    altInput: true,
                    altFormat: "F j, Y",
                });
            })

            inputBirthday = flatpickr(birthdayExLarge, {
                enableTime: true,
                dateFormat: "Z", // Định dạng phù hợp với ISO 8601
                altInput: true,
                altFormat: "F j, Y h:i K",
                time_24hr: false
            });

        },
        modalInit() {
            modelEdit = new bootstrap.Modal(modalEditElement)
            modelConfirmationDelete = new bootstrap.Modal(modalConfirmationDeleteElement)
            modelRoleClaim = new bootstrap.Modal(modelRoleClaimElement)
            modalConfirmationDeleteRoleClaim = new bootstrap.Modal(modalConfirmationDeleteRoleClaimElement)
        },
        tableInit() {
            tableUsers = new DataTable('#js-table-users', {
                responsive: true,
                dom: '<"card-header flex-column flex-md-row"<"head-label text-center"><"dt-action-buttons text-end pt-3 pt-md-0"B>><"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6 d-flex justify-content-center justify-content-md-end"f>>t<"row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>', // B: Buttons, f: Filter input, r: Processing, t: Table, i: Table information, p: Pagination
                buttons: [{
                    extend: "collection",
                    className: "btn btn-label-primary dropdown-toggle me-2",
                    text: '<i class="bx bx-export me-sm-1"></i> <span class="d-none d-sm-inline-block">Export</span>',
                    buttons: [{
                        extend: "print",
                        text: '<i class="bx bx-printer me-1" ></i>Print',
                        className: "dropdown-item",
                        title: 'User List', // Tiêu đề của tài liệu in
                        filename: function () {
                            var timestamp = Math.floor(Date.now() / 1000); // Lấy thời gian hiện tại tính bằng giây
                            return 'User List - ' + timestamp;
                        },
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9],
                            format: {
                                //body: function (e, t, a) {
                                //    var s;
                                //    return e.length <= 0 ? e : (e = $.parseHTML(e),
                                //        s = "",
                                //        $.each(e, function (e, t) {
                                //            void 0 !== t.classList && t.classList.contains("user-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
                                //        }),
                                //        s)
                                //}
                                body: function (data, row, column, node) {
                                    // Trả về giá trị dữ liệu của cột
                                    return (column === 3 || column === 1) ? ($(node).attr('data-original-value') || data) : ($(node).find(':input').val() || data)
                                }

                            }
                        },
                        customize: function (e) {
                            $(e.document.body).css("color", config.colors.headingColor).css("border-color", config.colors.borderColor).css("background-color", config.colors.bodyBg),
                                $(e.document.body).find("table").addClass("compact").css("color", "inherit").css("border-color", "inherit").css("background-color", "inherit")
                        }
                    }, {
                        extend: "csv",
                        text: '<i class="bx bx-file me-1" ></i>Csv',
                        className: "dropdown-item",
                        title: 'User List', // Tiêu đề của tài liệu in
                        filename: function () {
                            var timestamp = Math.floor(Date.now() / 1000); // Lấy thời gian hiện tại tính bằng giây
                            return 'User List - ' + timestamp;
                        },
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9],
                            format: {
                                //body: function (e, t, a) {
                                //    var s;
                                //    return e.length <= 0 ? e : (e = $.parseHTML(e),
                                //        s = "",
                                //        $.each(e, function (e, t) {
                                //            void 0 !== t.classList && t.classList.contains("user-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
                                //        }),
                                //        s)
                                //}

                                body: function (data, row, column, node) {
                                    // Trả về giá trị dữ liệu của cột
                                    return (column === 3 || column === 1) ? ($(node).attr('data-original-value') || data) : ($(node).find(':input').val() || data)
                                }
                            }
                        }
                    }, {
                        extend: "excel",
                        text: '<i class="bx bxs-file-export me-1"></i>Excel',
                        className: "dropdown-item",
                        title: 'User List', // Tiêu đề của tài liệu in
                        filename: function () {
                            var timestamp = Math.floor(Date.now() / 1000); // Lấy thời gian hiện tại tính bằng giây
                            return 'User List - ' + timestamp;
                        },
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9],
                            format: {
                                //body: function (e, t, a) {
                                //    var s;
                                //    return e.length <= 0 ? e : (e = $.parseHTML(e),
                                //        s = "",
                                //        $.each(e, function (e, t) {
                                //            void 0 !== t.classList && t.classList.contains("user-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
                                //        }),
                                //        s)
                                //}
                                body: function (data, row, column, node) {
                                    // Trả về giá trị dữ liệu của cột
                                    return (column === 3 || column === 1) ? ($(node).attr('data-original-value') || data) : ($(node).find(':input').val() || data)
                                }
                            }
                        }
                    }, {
                        extend: "pdf",
                        text: '<i class="bx bxs-file-pdf me-1"></i>Pdf',
                        className: "dropdown-item",
                        title: 'User List', // Tiêu đề của tài liệu in
                        filename: function () {
                            var timestamp = Math.floor(Date.now() / 1000); // Lấy thời gian hiện tại tính bằng giây
                            return 'User List - ' + timestamp;
                        },
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9],
                            format: {
                                //body: function (e, t, a) {
                                //    var s;
                                //    return e.length <= 0 ? e : (e = $.parseHTML(e),
                                //        s = "",
                                //        $.each(e, function (e, t) {
                                //            void 0 !== t.classList && t.classList.contains("user-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
                                //        }),
                                //        s)
                                //}
                                body: function (data, row, column, node) {
                                    // Trả về giá trị dữ liệu của cột
                                    return (column === 3 || column === 1) ? ($(node).attr('data-original-value') || data) : ($(node).find(':input').val() || data)
                                }
                            }
                        }
                    }, {
                        extend: "copy",
                        text: '<i class="bx bx-copy me-1" ></i>Copy',
                        className: "dropdown-item",
                        title: 'User List', // Tiêu đề của tài liệu in
                        filename: function () {
                            var timestamp = Math.floor(Date.now() / 1000); // Lấy thời gian hiện tại tính bằng giây
                            return 'User List - ' + timestamp;
                        },
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9],
                            format: {
                                //body: function (e, t, a) {
                                //    var s;
                                //    return e.length <= 0 ? e : (e = $.parseHTML(e),
                                //        s = "",
                                //        $.each(e, function (e, t) {
                                //            void 0 !== t.classList && t.classList.contains("user-name") ? s += t.lastChild.firstChild.textContent : void 0 === t.innerText ? s += t.textContent : s += t.innerText
                                //        }),
                                //        s)
                                //}
                                body: function (data, row, column, node) {
                                    // Trả về giá trị dữ liệu của cột
                                    return (column === 3 || column === 1) ? ($(node).attr('data-original-value') || data) : ($(node).find(':input').val() || data)
                                }
                            }
                        }
                    }]
                }, {
                    text: '<i class="bx bx-plus me-sm-1"></i> <span class="d-none d-sm-inline-block">Add New Record</span>',
                    className: "create-new btn btn-primary"
                }],

                columnDefs: [{
                    targets: 4, // index của cột "Address"
                    render: function (data, type, row) {
                        if (type === 'display') {
                            return data.length > 25 ? data.substr(0, 25) + '...' : data;
                        }
                        return data;
                    }
                }, {
                    targets: 2, // index của cột "Email"
                    render: function (data, type, row) {
                        if (type === 'display') {
                            return data.length > 20 ? data.substr(0, 20) + '...' : data;
                        }
                        return data;
                    }
                }]

            })
            tableClaim = new DataTable(tableClaimElement, {
                responsive: true,
                dom: '<"card-header flex-column flex-md-row"<"head-label text-center"><"dt-action-buttons text-end pt-3 pt-md-0"B>><"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6 d-flex justify-content-center justify-content-md-end"f>>t<"row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>', // B: Buttons, f: Filter input, r: Processing, t: Table, i: Table information, p: Pagination
                buttons: [
                    {
                        extend: "collection",
                        className: "btn btn-label-primary dropdown-toggle me-2",
                        text: '<i class="bx bx-export me-sm-1"></i> <span class="d-none d-sm-inline-block">Export</span>',
                        buttons: [
                            this.exportButtonConfig("print", "bx bx-printer me-1", "Print","User Claim"),
                            this.exportButtonConfig("csv", "bx bx-file me-1", "Csv", "User Claim"),
                            this.exportButtonConfig("excel", "bx bxs-file-export me-1", "Excel", "User Claim"),
                            this.exportButtonConfig("pdf", "bx bxs-file-pdf me-1", "Pdf", "User Claim"),
                            this.exportButtonConfig("copy", "bx bx-copy me-1", "Copy", "User Claim")
                        ]
                    },
                    {
                        text: '<i class="bx bx-plus me-sm-1"></i> <span class="d-none d-sm-inline-block">Add Claim</span>',
                        className: "create-new-role-claim btn btn-primary"
                    }
                ]
            })
        },
        exportButtonConfig(type, iconClass, text, title) {          
            return {
                extend: type,
                text: `<i class="${iconClass}"></i>${text}`,
                className: "dropdown-item",
                title,
                filename: () => `${title} - ${Math.floor(Date.now() / 1000)}`,
                exportOptions: {
                    columns: [0, 1],
                    format: {
                        body: (data, row, column, node) => (column === 3 || column === 1) ? ($(node).attr('data-original-value') || data) : ($(node).find(':input').val() || data)
                    }
                }
            };
        },
        addClaimRowInTable(array,table) {
            table.clear().draw();
            array && array?.length > 0 && array?.forEach(item =>
                table.row.add([
                    item.type,
                    item.value,
                    `<div class="dropdown">
                    <button type="button" class="p-0 btn btn-primary btn-icon rounded-pill dropdown-toggle hide-arrow" data-bs-toggle="dropdown">
                        <i class="bx bx-dots-vertical-rounded"></i>
                    </button>
                    <div class="dropdown-menu">
                        <button class="dropdown-item js-action-claim-edit" type="button" data-type="${item.type}" data-value="${item.value}" data-claim-id="${item.id}">
                            <i class="bx bx-edit-alt me-1"></i> Edit
                        </button>
                        <button class="dropdown-item js-action-claim-delete" type="button" data-type="${item.type}" data-value="${item.value}"  data-claim-id="${item.id}">
                            <i class="bx bx-trash me-1"></i> Delete
                        </button>
                    </div>
                </div>`
                ]).draw(false)
            )
            setTimeout(() => {
                this.resetCreateNewClaim();
            }, 200);
        },
        loadingConfig() { 
            // Configure NProgress
            NProgress.configure({
                minimum: 0.1, // Minimum percentage to show
                easing: 'ease', // CSS easing string
                speed: 500, // Animation speed in ms
                trickle: true, // Enable trickling of progress
                trickleSpeed: 200, // How often to trickle, in ms
                showSpinner: true, // Show spinner
                parent: 'body' // Element to append progress bar to
            });

            // Customizing and creating a Spinner
            loading = new Loading({
                lines: 13, // The number of lines to draw
                length: 38, // The length of each line
                width: 17, // The line thickness
                radius: 45, // The radius of the inner circle
                scale: 0.65, // Scales overall size of the spinner
                corners: 1, // Corner roundness (0..1)
                speed: 1, // Rounds per second
                rotate: 0, // The rotation offset
                animation: 'spinner-line-fade-quick', // The CSS animation name for the lines
                direction: 1, // 1: clockwise, -1: counterclockwise
                color: '#5f61e6', // CSS color or array of colors
                fadeColor: 'white', // CSS color or array of colors
                top: '50%', // Top position relative to parent
                left: '50%', // Left position relative to parent
                shadow: '0 0 1px transparent', // Box-shadow for the lines
                zIndex: 2000000000, // The z-index (defaults to 2e9)
                className: 'spinner', // The CSS class to assign to the spinner
                position: 'absolute', // Element positioning
            })
        },
        async callRolesEdit() {
            try {
                const res = await apiGetRoles()              
                if (res?.status) {
                    accordionRolesBody.innerHTML = ""
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

                        accordionRolesBody.appendChild(checkbox);
                        accordionRolesBody.appendChild(label);
                        accordionRolesBody.appendChild(document.createElement('br'));
                    });
                }
            } catch (error) {
                ConsoleErrorCatch(error)
            }
        },
        getCheckedRoles() {
            const checkboxes = document.querySelectorAll('input[type="checkbox"][name="roles"]');
            const checkedRoles = [];

            checkboxes.forEach(checkbox => {
                if (checkbox.checked) {
                    checkedRoles.push(checkbox.dataset.name); // Thêm giá trị của checkbox đã chọn vào mảng
                }
            });

            return checkedRoles;
        },
        resetCreateNew() {
            const e = document.querySelector(".create-new")
                , t = document.querySelector("#add-new-record");
            e && e.addEventListener("click", function () {
                offCanvasEl = new bootstrap.Offcanvas(t),
                    t.querySelector(".dt-full-name").value = "",
                    t.querySelector(".dt-post").value = "",
                    t.querySelector(".dt-email").value = "",
                    t.querySelector(".dt-date").value = "",
                    t.querySelector(".dt-salary").value = "",
                    offCanvasEl.show()
            })
        },
        resetCreateNewClaim() {
            const e = document.querySelector(".create-new-role-claim")
            btnTableRoleClaimEdits = document.querySelectorAll(".js-action-claim-edit")
            btnTableRoleClaimDeletes = document.querySelectorAll(".js-action-claim-delete")
            this.addEventBtnRoleClaimAction()


            e && e.addEventListener("click", function () {
                inputRoleType.value = "",
                inputRoleValue.value = "",
                btnModalRoleClaim.dataset.type = "add",
                titleModalRoleClaim.innerText = "Add Claim"
                modelRoleClaim.show()
            })
        },
        addEventBtnRoleClaimAction() {
            Array.from(btnTableRoleClaimEdits).forEach(btn => {
                btn.addEventListener("click", async (e) => {                  
                    btnModalRoleClaim.dataset.type = "edit"
                    titleModalRoleClaim.innerText = "Edit Claim"
                    const btnElement = e.currentTarget
                    await loading.show()
                    NProgress.start()
                    try {
                        const res = await apiGetClaimUser(btnElement.dataset.claimId)
                        if (res?.status) {
                            inputRoleType.value = res?.data?.type
                            inputRoleValue.value = res?.data?.value
                            btnModalRoleClaim.dataset.claimId = res?.data?.id
                            modelRoleClaim.show()
                        } else {
                            toast.danger(res?.errors?.map(str => `- ${str}`).join("\n"))
                            ConsoleErrorStatus(res?.errors)
                        }
                    } catch (error) {
                        ConsoleErrorCatch(error)
                    } finally {
                        // Ẩn modal sau khi hoàn thành tất cả các xử lý (bất kể thành công hay thất bại)
                        loading.hide()
                        NProgress.done()
                    }
                })
            })

            Array.from(btnTableRoleClaimDeletes).forEach(btn => {
                btn.addEventListener("click", (e) => {
                    const btnElement = e.currentTarget
                    console.log(btnElement.dataset.claimId)
                    btnConfirmationDeleteRoleClaim.dataset.claimId = btnElement.dataset.claimId              
                    titleConfirmationDeleteRoleClaim.innerText = `Delete ID: ${btnElement.dataset.claimId}`
                    modalConfirmationDeleteRoleClaim.show()
                })
            })




        }, 
        addEvents() {
            Array.from(btnTableDeletes).forEach(btn => {
                btn.addEventListener("click", (e) => {                  
                    inputIdUserDelete.value = e.currentTarget.dataset.id
                    modelConfirmationDelete.show()
                })
            })
            Array.from(btnTableEdits).forEach(btn => {
                btn.addEventListener("click", async (e) => {                  
                    const btnElement = e.currentTarget         
                    IdUserLogin = btnElement.dataset.id
                    try {
                        await loading.show()
                        NProgress.start()
                        const res = await apiGetUserById(btnElement.dataset.id)
                        if (res?.status) {
                            console.log(res)
                            const checkboxes = document.querySelectorAll('input[type="checkbox"][name="roles"]');
                            const { avatar, displayName, gender, id, birthday, roles, claims } = res?.data
                            btnModalEdit.dataset.id = id
                            uploadedAvatar.src = avatar
                            displayExLarge.value = displayName
                            genderExLarge.value = gender ?? "other"
                            this.addClaimRowInTable(claims, tableClaim)
                            checkboxes.forEach(checkbox => {
                                checkbox.checked = roles.includes(checkbox.value)
                            });
                            inputBirthday.setDate(birthday, true);                           
                            modelEdit.show()
                        } else {
                            ConsoleErrorStatus(res?.errors)
                        }
                    } catch (error) {
                        ConsoleErrorCatch(error)
                    } finally {
                        // Ẩn modal sau khi hoàn thành tất cả các xử lý (bất kể thành công hay thất bại)
                        loading.hide()
                        NProgress.done()
                    }
                })
            })

            btnModalEdit.addEventListener("click", async () => {
                if (displayExLarge.value === '' || genderExLarge.value === '') {
                    toast.warning('Missing input !!');
                    return
                }
                const formData = new FormData()
                formData.append('id', btnModalEdit.dataset.id); // Ví dụ: id của người dùng
                formData.append('displayName', displayExLarge.value);
                formData.append('gender', genderExLarge.value);
                formData.append('birthday', inputBirthday.input.value);
                const rolesArray = this.getCheckedRoles()

                if (rolesArray.length > 0) {
                    rolesArray.forEach(role => {
                        formData.append('roles', role);
                    })
                } else {
                    formData.append('roles', rolesArray);
                }
                
                
                
                if (upload.files[0]) {
                    formData.append('avatar', upload.files[0]); // avatarFile là file hình ảnh
                } else {
                    console.log(upload.files[0])
                }




                console.log(btnModalEdit.dataset.id)
                console.log(uploadedAvatar.src)
                console.log(displayExLarge.value)
                console.log(genderExLarge.value)
                console.log(inputBirthday.input.value)
                console.log(upload.files[0])
               
                try {
                    await loading.show()
                    NProgress.start()
                    const res = await apiEditUser(formData, btnModalEdit.dataset.id)
                    if (res?.status) {
                        toast.success(res?.message);
                        location.reload();
                    } else {
                        ConsoleErrorStatus(res?.errors)
                        toast.danger(res?.message);                       
                    }
                } catch (error) {
                    ConsoleErrorCatch(error)
                    toast.danger(error?.message);
                } finally {
                    // Ẩn modal sau khi hoàn thành tất cả các xử lý (bất kể thành công hay thất bại)
                    loading.hide()
                    NProgress.done()
                }
            })


            btnModalRoleClaim.addEventListener("click", async () => {
                if (inputRoleType.value == "" || inputRoleValue.value == "") {
                    toast.warning("Missing input !!")
                    return
                }

                if (btnModalRoleClaim.dataset.type == "add") {
                    console.log("UserId : ", IdUserLogin)
                    console.log("ClaimType : ", inputRoleType.value)
                    console.log("ClaimValue : ", inputRoleValue.value)
                    await loading.show()
                    NProgress.start()
                    try {
                        const res = await apiCreateClaimUser({ userId: IdUserLogin, claimType: inputRoleType.value, claimValue: inputRoleValue.value })
                        if (res?.status) {
                            toast.success(res?.message)                           
                            this.addClaimRowInTable(res?.data?.claims, tableClaim)
                            modelRoleClaim.hide()
                        } else {
                            toast.danger(res?.errors?.map(str => `- ${str}`).join("\n"))
                            ConsoleErrorStatus(res?.errors)
                        }
                    } catch (error) {
                        ConsoleErrorCatch(error)
                    } finally {
                        // Ẩn modal sau khi hoàn thành tất cả các xử lý (bất kể thành công hay thất bại)
                        loading.hide()
                        NProgress.done()
                    }
                } else {
                    await loading.show()
                    NProgress.start()
                    try {
                        const res = await apiEditClaimUser({ userId: IdUserLogin, claimId: btnModalRoleClaim.dataset.claimId, claimType: inputRoleType.value, claimValue: inputRoleValue.value })
                        if (res?.status) {
                            toast.success(res?.message)
                            this.addClaimRowInTable(res?.data?.claims, tableClaim)
                            modelRoleClaim.hide()
                        } else {
                            toast.danger(res?.errors?.map(str => `- ${str}`).join("\n"))
                            ConsoleErrorStatus(res?.errors)
                        }
                    } catch (error) {
                        ConsoleErrorCatch(error)
                    } finally {
                        // Ẩn modal sau khi hoàn thành tất cả các xử lý (bất kể thành công hay thất bại)
                        loading.hide()
                        NProgress.done()
                    }
                }
            })



            btnConfirmationDeleteRoleClaim.addEventListener("click", async (e) => {
                const btnElement = e.currentTarget
                await loading.show()
                NProgress.start()
                try {
                    const res = await apiDeleteClaimUser(btnElement.dataset.claimId, { userId: IdUserLogin, claimId: btnElement.dataset.claimId })
                    if (res?.status) {
                        toast.success(res?.message)
                        this.addClaimRowInTable(res?.data?.claims, tableClaim)
                        modalConfirmationDeleteRoleClaim.hide()
                    } else {
                        toast.danger(res?.errors?.map(str => `- ${str}`).join("\n"))
                        ConsoleErrorStatus(res?.errors)
                    }
                } catch (error) {
                    ConsoleErrorCatch(error)
                } finally {
                    // Ẩn modal sau khi hoàn thành tất cả các xử lý (bất kể thành công hay thất bại)
                    loading.hide()
                    NProgress.done()
                }

            })
        }
    }
    return managerUserStore
})()





export default managerUser