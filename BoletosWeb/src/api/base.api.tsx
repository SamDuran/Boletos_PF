import axios from "axios"

export const BASE_URL = "https://localhost:7257/"

export const instance = axios.create({
    baseURL: BASE_URL
})