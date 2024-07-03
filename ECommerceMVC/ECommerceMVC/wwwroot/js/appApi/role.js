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



export const apiCreateRoleClaim = (data) => axios({
    url: '/Admin/ManagerUsers/CreateRoleClaim',
    method: 'post',
    data
})


export const apiEditRoleClaim = (data) => axios({
    url: '/Admin/ManagerUsers/EditRoleClaim',
    method: 'put',
    data
})


export const apiDeleteRoleClaim = (claimId,data) => axios({
    url: '/Admin/ManagerUsers/DeleteRoleClaim/' + claimId,
    method: 'delete',
    data
})

export const apiGetClaimDetails = (claimId) => axios({
    url: '/Admin/ManagerUsers/GetRoleClaimById/' + claimId,
    method: 'get',
})

