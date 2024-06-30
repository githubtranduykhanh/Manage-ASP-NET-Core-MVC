import axios from '../axiosConfig.js';


export const apiCreateRole = (data) => axios({
    url: '/Admin/ManagerUsers/CreateRole',
    method: 'post',
    data,
})


export const apiGetRoleById = (rid) => axios({
    url: '/Admin/ManagerUsers/GetRoleById/' + rid,
    method: 'get',
})


export const apiEditRole = (rid,data) => axios({
    url: '/Admin/ManagerUsers/EditRole/' + rid,
    method: 'post',
    data
})


export const apiGetRoles = () => axios({
    url: '/Admin/ManagerUsers/GetRoles',
    method: 'get',
})


