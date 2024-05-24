import { GETALLPRODUCT } from "./types.js"
const initialState = {
    listProduct: []
};

const productReducer = (state = initialState, action) => {
    switch (action.type) {
        case GETALLPRODUCT:
            return ({ ...state, listProduct: [...action.payload] });
        default:
            return state;
    }
};

export default productReducer;