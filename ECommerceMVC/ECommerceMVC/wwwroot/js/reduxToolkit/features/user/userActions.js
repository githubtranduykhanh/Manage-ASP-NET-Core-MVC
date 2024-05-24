import { apiGetUsers } from "../../../appApi/user.js"
import { createAsyncThunk } from "https://cdn.jsdelivr.net/npm/@reduxjs/toolkit@2.2.5/+esm"


export const getAllUserAsync = createAsyncThunk(
    'user/getall',
    async (data, { rejectWithValue }) => {       
        try {
            const response = await apiGetUsers()
            if (!response?.status) return rejectWithValue(response)
            return response?.data
        } catch (err) {
            // Use `err.response.data` as `action.payload` for a `rejected` action,
            // by explicitly returning it using the `rejectWithValue()` utility
            return rejectWithValue(err.response.data)
        }
    },
)