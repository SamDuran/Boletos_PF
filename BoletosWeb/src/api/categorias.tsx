import { instance } from "./base.api";
import "../../models/Categoria.tsx"

const endpoint = 'api/categorias';

export const categorias = {
    getAll: function(){
        return instance.get(endpoint)
    }
}