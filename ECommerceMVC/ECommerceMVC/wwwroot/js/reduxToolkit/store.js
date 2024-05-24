import { configureStore } from "https://cdn.jsdelivr.net/npm/@reduxjs/toolkit@2.2.5/+esm"
import userReducer from "./features/user/userSlice.js"


const store = configureStore({
    reducer: {       
        user: userReducer
    },
})

export default store