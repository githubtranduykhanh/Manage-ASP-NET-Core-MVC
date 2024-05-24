import { UPDATE, GETALL} from "./types.js"
const initialState = {
    user:{},
    listUser:[]
};

const userReducer = (state = initialState, action) => {
    switch (action.type) {       
        case UPDATE:
            return ({ ...state, user: {...action.payload} });
        case GETALL:
            return { ...state, listUser: [...action.payload]};    
        default:
            return state;
    }
};

export default userReducer;
