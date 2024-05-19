﻿import { apiLoginUser,apiRegisterUser } from "./appApi/user.js"

document.addEventListener("DOMContentLoaded", () => {
    const accout = (() => {
        const localStorage = window.localStorage
        const container = document.getElementById('container')
        const btns = document.getElementsByClassName('js-display-form-toggle')
        const btnSingInWithPhone = document.getElementById("js-btn-show-phone-form-login")
        const btnOTP = document.getElementById("js-btn-send-otp")
        const btnPhone = document.getElementById("js-btn-send-phone")
        const btnBackToPhone = document.getElementById("js-btn-back-to-phone")
        const btnSignIn = document.getElementById("js-btn-sign-in")
        const btnSignUp = document.getElementById("js-btn-sign-up")
        const forms = document.querySelectorAll("form")
        const formSingUp = document.getElementById("js-form-sign-up")
        const formPhone = document.getElementById("js-form-sign-in-phone")
        const formOTP = document.getElementById("js-form-sign-in-otp")
        const formSignIn = document.getElementById("js-form-sign-in")
        const inputSignInEmailOrPhone = document.getElementById("js-input-sign-in-email-or-phone")
        const inputSignInPassword = document.getElementById("js-input-sign-in-password")
        const textSingUpOrPhone = document.getElementById("js-text-sign-up-or-phone")
        const accoutStore = {
            addEventDisplayForm() {
                Array.from(btns).forEach(btn => {
                    btn.addEventListener("click", () => {
                        container.classList.toggle('sign-in')
                        container.classList.toggle('sign-up')
                        this.resetFormDefault();
                    })
                })
                btnSingInWithPhone.addEventListener("click", () => {
                    formSingUp.classList.toggle("close")
                    formOTP.classList.toggle("close")
                    textSingUpOrPhone.innerText = "Eeceive OTP from Phone"
                    container.classList.toggle('sign-in')
                    container.classList.toggle('sign-up')
                })
                btnPhone.addEventListener("click", () => {
                    formPhone.classList.toggle("close")
                    formOTP.classList.toggle("close")
                    textSingUpOrPhone.innerText = "Send OTP"
                })
                btnBackToPhone.addEventListener("click", () => {
                    formPhone.classList.toggle("close")
                    formOTP.classList.toggle("close")
                    textSingUpOrPhone.innerText = "Eeceive OTP from Phone"
                })
                btnSignIn.addEventListener("click", async () => {
                    if (formSignIn.checkValidity()) {
                        this.btnSetting(btnSignIn, 'Loading...', true, true)
                        const res = await apiLoginUser({ emailorphone: inputSignInEmailOrPhone.value, passwordlogin: inputSignInPassword.value })
                        this.btnSetting(btnSignIn, 'Sign in', false, false)
                        console.log("Login res :", res)
                        if (res?.status) {
                            localStorage.removeItem('accessToken')
                            localStorage.setItem('accessToken', res?.data?.accessToken)
                            toastr.success(res?.mes)
                        } else {
                            toastr.error(res?.mes)
                        }
                    }
                    formSignIn.classList.add('was-validated');                   
                })
                btnSignUp.addEventListener("click", async () => {
                    if (formSingUp.checkValidity()) {
                        try {
                            const formSingUpData = document.getElementById("js-form-sign-up")
                            var formData = new FormData(formSingUpData);
                            console.log([...formData.entries()])
                            // Tạo một đối tượng JavaScript mới để lưu trữ dữ liệu
                            const formDataObj = {};

                            // Lặp qua tất cả các cặp key-value trong FormData
                            for (const [key, value] of formData.entries()) {
                                // Thêm các cặp key-value vào đối tượng JavaScript mới
                                formDataObj[key] = value;
                            }
                            this.btnSetting(btnSignUp, 'Loading...', true, true)
                            const res = await apiRegisterUser(formDataObj)
                            this.btnSetting(btnSignUp, 'Sign up', false, false)
                            console.log("Login res :", res)
                            if (res?.status) {
                                toastr.success(res?.mes)
                            } else {
                                toastr.error(res?.mes)
                            }
                        } catch (e) {
                            console.error(e)
                        }                      
                    }
                    formSingUp.classList.add('was-validated');
                })
                setTimeout(() => {
                    container.classList.add('sign-in')
                }, 200)
            },
            resetInputInForm(form) {
                const inputs = form.querySelectorAll('input')
                Array.from(inputs).forEach(input => {
                    if (input.name != 'loginType') input.value = ''          
                })
            },
            resetFormDefault() {
                console.log("resetFormDefault")
                formSingUp.classList.remove("close")
                formOTP.classList.add("close")
                formPhone.classList.add("close")
                textSingUpOrPhone.innerText = "Join with us"
                Array.from(forms).forEach(form => {
                    form.classList.remove("was-validated")
                    this.resetInputInForm(form)
                })              
            },         
            showLoadingModal(id) {
                console.log("show")
                $(`#${id}`).modal('show')
            },
            hidenLoadingModal(id) {
                console.log("hiden")
                $(`#${id}`).modal('hide')
            },
            btnSetting(btn, mes, disabled = false, isLoading = false) {             
                btn.disabled = disabled
                if (isLoading) {
                    const html = `<span class="spinner-border spinner-border-sm mr-3" role="status" aria-hidden="true"></span>${mes}`
                    btn.innerHTML = html
                    return
                }
                btn.innerText = mes
            },
            configToastr() {
                toastr.options = {
                    "closeButton": true,
                    "debug": true,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
                    "preventDuplicates": false,
                    "onclick": null,
                    "showDuration": "300",
                    "hideDuration": "1000",
                    "timeOut": "3000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                }
            },
            innit() {
                this.configToastr()
                this.addEventDisplayForm()
            },
        }
        return accoutStore
    })()
    accout.innit()
})

