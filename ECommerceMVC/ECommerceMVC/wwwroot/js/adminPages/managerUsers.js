import { apiGetUserById,apiEditUser } from "../appApi/user.js"
import { ConsoleErrorCatch, ConsoleErrorStatus } from "../helper/HelperError.js"
import { ToastBody } from "../helper/HelperToasts.js"

import { formatDatatime } from "../helper/HelperString.js"

const managerUser = (() => {
    let tableUsers, toast, modelEdit, modelConfirmationDelete, offCanvasEl, inputBirthday
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


    const managerUserStore = {
        init() {
            modelEdit = new bootstrap.Modal(modalEditElement)
            modelConfirmationDelete = new bootstrap.Modal(modalConfirmationDeleteElement)
            toast = new ToastBody({ placement: 'top-0 end-0', title: 'E Commerce', icon:'<i class="bx bx-bell me-2"></i>', toastOptions: { delay: 2000, autohide: true } })
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
                            columns: [1,2,3, 4, 5, 6, 7,8,9],
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
            const headLabel = document.querySelector('div.head-label')
            headLabel.innerHTML = '<h5 class="card-title mb-0">DataTable User</h5>'
            setTimeout(() => {
                this.resetCreateNew()
            }, 200) 
            const dataTimeInputs = document.querySelectorAll(".dt-date")
            Array.from(dataTimeInputs).forEach(input => { 
                flatpickr(input, {
                    enableTime: true,
                    dateFormat: "Y-m-d H:i",
                    time_24hr: false, // Sử dụng định dạng 12 giờ (AM/PM), đổi thành true nếu muốn 24 giờ
                    /*monthSelectorType: "static",*/
                    defaultDate: "2024-06-28 02:34",
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
            

            this.addEvents()
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
        addEvents() {
            Array.from(btnTableDeletes).forEach(btn => {
                btn.addEventListener("click", (e) => {
                    console.log(e.currentTarget.dataset.id)
                    inputIdUserDelete.value = e.currentTarget.dataset.id
                    modelConfirmationDelete.show()
                })
            })
            Array.from(btnTableEdits).forEach(btn => {
                btn.addEventListener("click", async (e) => {
                    console.log(e.currentTarget.dataset.id)
                    try {
                        const res = await apiGetUserById(e.currentTarget.dataset.id)
                        if (res?.status) {
                            console.log(res?.data)
                            const { avatar, displayName, gender, id, birthday } = res?.data
                            btnModalEdit.dataset.id = id
                            uploadedAvatar.src = avatar
                            displayExLarge.value = displayName
                            genderExLarge.value = gender ?? "other"
                            

                            inputBirthday.setDate(birthday, true);

                            console.log(formatDatatime(birthday))
                            modelEdit.show()
                        } else {
                            ConsoleErrorStatus(res?.errors)
                        }
                    } catch (error) {
                        ConsoleErrorCatch(error)
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
                }
            })
        }
    }
    return managerUserStore
})()





export default managerUser