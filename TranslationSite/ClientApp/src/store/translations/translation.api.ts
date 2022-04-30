import {createApi, fetchBaseQuery} from '@reduxjs/toolkit/query/react'
import { ILanguage } from './translation.types'

export const translationApi = createApi({
    reducerPath: 'api/translations',
    baseQuery: fetchBaseQuery({
        baseUrl: '/Translation'
    }),
    endpoints: build => ({
        getLanguages: build.query<ILanguage[], number>({query: (limit = 20) => '/Languages'}),
        translate: build.mutation<string[], FormData>({      
            query: (request) => ({
            url: '/Translate',
            method: 'POST',
            body: request,
          })
        })
    })
})

export const { useGetLanguagesQuery, useTranslateMutation } = translationApi