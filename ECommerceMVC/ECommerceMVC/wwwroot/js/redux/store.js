import { createStore, combineReducers } from 'https://cdn.jsdelivr.net/npm/redux@4.1.2/es/redux.mjs';
import { isEqual } from 'https://cdn.skypack.dev/lodash-es';
import userReducer from "./features/user/reducer.js"
import productReducer from "./features/product/reducer.js"



const rootReducer = combineReducers({
    user: userReducer,
    product: productReducer
});

const store = createStore(rootReducer);


export const subscribeToStore = (callback) => {
    let prevState = store.getState();
    store.subscribe(() => {
        const newState = store.getState();
        if (!isEqual(newState, prevState)) {
            callback(newState, prevState);
            prevState = newState;
        }
    });
};


export default store;