import { createSlice } from "https://cdn.jsdelivr.net/npm/@reduxjs/toolkit@2.2.5/+esm";
import * as actions from "./userActions.js"

const userSlice = createSlice({
    name: 'user',
    initialState: {
        listUser: [],
        isLoading: false,
        errorMessage: ''
    },
    reducers: {
    },
    extraReducers: (builder) => {
        builder.addCase(actions.getAllUserAsync.pending, (state, action) => {
            state.isLoading = true;
        })
        builder.addCase(actions.getAllUserAsync.fulfilled, (state, action) => {
            state.isLoading = false;          
            state.listUser = action.payload
        })
        builder.addCase(actions.getAllUserAsync.rejected, (state, action) => {
            console.log(action)
            state.isLoading = false;
            state.errorMessage = action.error.message;
        })
    }
})


export default userSlice.reducer

