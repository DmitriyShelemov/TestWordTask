import { bindActionCreators } from "@reduxjs/toolkit"
import { useDispatch } from "react-redux"
import { translatedActions } from "../store/translations/translated.slice"

const allActions = {
    ...translatedActions
}

export const useActions = () => {
    const dispatch = useDispatch()
    return bindActionCreators(allActions, dispatch)
}