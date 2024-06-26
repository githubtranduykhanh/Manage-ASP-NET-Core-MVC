import axios from '../axiosConfig.js';


export const apiRegister = (data) => axios({
    url: '/user/register',
    method: 'post',
    data,
    withCredentials: true //Lưu cookie trên trình duyệt cùng với cấu hình credentials:true server
})


export const apiGetUsers = () => axios({
    url: 'api/v1/user/list',
    method: 'get',
})


export const apiGetUser = (params) => axios({
    url: '/user',
    method: 'get',
    params
})


export const apiUpdateUser = (data, uid) => axios({
    url: '/user/' + uid,
    method: 'put',
    data
})


export const apiDeleteUser = (uid) => axios({
    url: '/user/' + uid,
    method: 'delete',
})


export const apiGetCurrentUser = () => axios({
    url: 'Account/CurrentUser',
    method: 'get',
})


export const apiRegisterUser = (data) => axios({
    url: '/Account/Register',
    method: 'post',
    data,
    withCredentials: true //Lưu cookie trên trình duyệt cùng với cấu hình credentials:true server
})

export const apiLoginUser = (data) => axios({
    url: '/Account/Login',
    method: 'post',
    data,
    withCredentials: true, //Lưu cookie trên trình duyệt cùng với cấu hình credentials:true server
})


export const apiGetUserById = (uid) => axios({
    url: '/Admin/ManagerUsers/GetUserById/' + uid,
    method: 'get',
})


export const apiEditUser = (data, uid) => axios({
    url: '/Admin/ManagerUsers/EditUser' + uid,
    method: 'post',
    data
})


