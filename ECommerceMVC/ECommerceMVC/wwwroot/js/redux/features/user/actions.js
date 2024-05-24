import { UPDATE, GETALL, GETBYID } from "./types.js"
import { apiGetUsers } from "../../../appApi/user.js"



export const updateUser = () => ({ type: UPDATE });
export const getAllUser = (data) => ({ type: GETALL, payload: data });
export const getByIdUser = () => ({ type: GETBYID });


export const getAllUserAsync = () => (dispatch) => new Promise((resolve, reject) => {
    apiGetUsers().then((res) => {
        if (!res?.status) reject(res)
        dispatch(getAllUser(res.data))
        resolve(res)
    }).catch((error) => {
        reject(error)
    })
}) 