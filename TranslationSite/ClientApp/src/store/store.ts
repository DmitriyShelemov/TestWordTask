import { configureStore, getDefaultMiddleware } from "@reduxjs/toolkit";
import { translationApi } from "./translations/translation.api";
import { translatedReducer } from "./translations/translated.slice";

export const store = configureStore({
    reducer: { 
        [translationApi.reducerPath]: translationApi.reducer,
        translated: translatedReducer
    },
    middleware: getDefaultMiddleware => getDefaultMiddleware().concat(translationApi.middleware),
})

export type TypeRootState = ReturnType<typeof store.getState>