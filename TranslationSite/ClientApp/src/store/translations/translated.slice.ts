import { createSlice, PayloadAction } from "@reduxjs/toolkit";

export interface ITranslations {
    texts: string[] | undefined;
}

const initialState:ITranslations = {} as ITranslations

export const translatedSlice = createSlice({
    name: 'translated',
    initialState,
    reducers: {
        setTexts: (state, action:PayloadAction<string[] | undefined>) => {
            state.texts = action.payload 
        }
    }
})

export const translatedReducer = translatedSlice.reducer
export const translatedActions = translatedSlice.actions