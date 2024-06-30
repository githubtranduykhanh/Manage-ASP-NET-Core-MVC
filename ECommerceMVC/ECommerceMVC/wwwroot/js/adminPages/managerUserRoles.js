import { apiCreateRole, apiGetRoleById, apiEditRole } from "../appApi/role.js"
import { ConsoleErrorCatch, ConsoleErrorStatus } from "../helper/HelperError.js"
import { ToastBody } from "../helper/HelperToasts.js"

import  Loading  from "../helper/HelperLoading.js"


import NProgress from 'https://cdn.jsdelivr.net/npm/nprogress@0.2.0/+esm'


const managerUserRole = (() => {
    let tableUsers, toast, modelEdit, modelConfirmationDelete, offCanvasEl, loading
    const modalEditElement = document.querySelector("#exLargeModalEdit")
    const modalConfirmationDeleteElement = document.querySelector("#confirmationDelete")
    const btnTableEdits = document.querySelectorAll(".js-action-edit")
    const btnTableDeletes = document.querySelectorAll(".js-action-delete")
    const btnModalEdit = document.querySelector("#js-btn-model-edit")
    const btnOffCanvasCreate = document.querySelector("#js-btn-offcanvas-create")
    const inputIdDelete = modalConfirmationDeleteElement.querySelector("#inputIdDelete")
    const inputNameRoleEdit = modalEditElement.querySelector("#nameRole")

   

    const managerStore = {
        init() {

            

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
                        title: 'Role List', // Tiêu đề của tài liệu in
                        filename: function () {
                            var timestamp = Math.floor(Date.now() / 1000); // Lấy thời gian hiện tại tính bằng giây
                            return 'Role List - ' + timestamp;
                        },
                        exportOptions: {
                            columns: [0, 1],
                            format: {                            
                                body: function (data, row, column, node) {
                                    // Trả về giá trị dữ liệu của cột
                                    return data                                  
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
                        title: 'Role List', // Tiêu đề của tài liệu in
                        filename: function () {
                            var timestamp = Math.floor(Date.now() / 1000); // Lấy thời gian hiện tại tính bằng giây
                            return 'Role List - ' + timestamp;
                        },
                        exportOptions: {
                            columns: [0, 1],
                            format: {
                           
                                body: function (data, row, column, node) {
                                    // Trả về giá trị dữ liệu của cột
                                    return  data
                                }
                            }
                        }
                    }, {
                        extend: "excel",
                        text: '<i class="bx bxs-file-export me-1"></i>Excel',
                        className: "dropdown-item",
                        title: 'Role List', // Tiêu đề của tài liệu in
                        filename: function () {
                            var timestamp = Math.floor(Date.now() / 1000); // Lấy thời gian hiện tại tính bằng giây
                            return 'Role List - ' + timestamp;
                        },
                        exportOptions: {
                            columns: [0, 1],
                            format: {
                               
                                body: function (data, row, column, node) {
                                    // Trả về giá trị dữ liệu của cột
                                    return data
                                }
                            }
                        }
                    }, {
                        extend: "pdf",
                        text: '<i class="bx bxs-file-pdf me-1"></i>Pdf',
                        className: "dropdown-item",
                        title: 'Role List', // Tiêu đề của tài liệu in
                        filename: function () {
                            var timestamp = Math.floor(Date.now() / 1000); // Lấy thời gian hiện tại tính bằng giây
                            return 'Role List - ' + timestamp;
                        },
                        exportOptions: {
                            columns: [0, 1],
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
                                    return data
                                }
                            }
                        }
                    }, {
                        extend: "copy",
                        text: '<i class="bx bx-copy me-1" ></i>Copy',
                        className: "dropdown-item",
                        title: 'Role List', // Tiêu đề của tài liệu in
                        filename: function () {
                            var timestamp = Math.floor(Date.now() / 1000); // Lấy thời gian hiện tại tính bằng giây
                            return 'Role List - ' + timestamp;
                        },
                        exportOptions: {
                            columns: [0, 1],
                            format: {
                               
                                body: function (data, row, column, node) {
                                    // Trả về giá trị dữ liệu của cột
                                    return data
                                }
                            }
                        }
                    }]
                }, {
                    text: '<i class="bx bx-plus me-sm-1"></i> <span class="d-none d-sm-inline-block">Add New Record</span>',
                    className: "create-new btn btn-primary"
                }],                                      
            })
            const headLabel = document.querySelector('div.head-label')
            headLabel.innerHTML = '<h5 class="card-title mb-0">DataTable User</h5>'
            setTimeout(() => {
                this.resetCreateNew()
            }, 200) 
                      
            this.addEvents()
        },
        resetCreateNew() {
            const e = document.querySelector(".create-new")
                , t = document.querySelector("#add-new-record");
            e && e.addEventListener("click", function () {
                offCanvasEl = new bootstrap.Offcanvas(t),
                    t.querySelector("#nameRoleAdd").value = "",                 
                    offCanvasEl.show()
            })
        },
        addEvents() {
            
            Array.from(btnTableDeletes).forEach(btn => {
                btn.addEventListener("click", (e) => {
                    console.log(e.currentTarget.dataset.id)
                    inputIdDelete.value = e.currentTarget.dataset.id
                    modelConfirmationDelete.show()
                })
            })
            Array.from(btnTableEdits).forEach(btn => {
                btn.addEventListener("click", async (e) => {          
                    const Id = e.currentTarget.dataset.id
                    console.log(Id)    
                    await loading.show()
                    NProgress.start()
                    try {
                        const res = await apiGetRoleById(Id)                    
                        if (res?.status) {
                            console.log(res?.data)
                            btnModalEdit.dataset.id = res?.data?.id
                            inputNameRoleEdit.value = res?.data?.name
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

            btnOffCanvasCreate.addEventListener("click", async () => {
                const inputRoleName = document.querySelector("#nameRoleAdd")
                if (inputRoleName.value === "") {
                    inputRoleName.focus()
                    toast.warning("Missing input !!")
                    return
                }
                console.log(inputRoleName.value)
                await loading.show()
                NProgress.start()
                try {

                    const res = await apiCreateRole({ roleName: inputRoleName.value })
                    if (res?.status) {
                        toast.success(res?.message)
                        setTimeout(() => {
                            location.reload()
                        },1800)
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


            btnModalEdit.addEventListener("click", async () => {
                if (inputNameRoleEdit.value === "") {
                    inputNameRoleEdit.focus()
                    toast.warning("Missing input !!")
                    return
                }
                await loading.show()
                NProgress.start()
                try {
                    const res = await apiEditRole(btnModalEdit.dataset.id, { roleId: btnModalEdit.dataset.id, roleName: inputNameRoleEdit.value })
                    if (res?.status) {
                        toast.success(res?.message)
                        setTimeout(() => {
                            location.reload()
                        }, 1800)
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
    return managerStore
})()





export default managerUserRole