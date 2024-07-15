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
    url: '/Admin/ManagerUsers/EditUser/' + uid,
    method: 'post',
    data
})


export const apiCreateClaimUser = (data) => axios({
    url: '/Admin/ManagerUsers/CreateUserClaim',
    method: 'post',
    data
})

export const apiGetClaimUser = (cid) => axios({
    url: '/Admin/ManagerUsers/GetUserClaim/' + cid,
    method: 'get',
})


export const apiEditClaimUser = (data) => axios({
    url: '/Admin/ManagerUsers/EditUserClaim',
    method: 'put',
    data
})


export const apiDeleteClaimUser = (claimId, data) => axios({
    url: '/Admin/ManagerUsers/DeleteUserClaim/' + claimId,
    method: 'delete',
    data
})



