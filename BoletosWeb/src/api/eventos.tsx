import { instance } from "./base.api";
import "../../models/Eventos.tsx"

const endpoint = 'api/eventos';

export const eventos = {
    getAll: function(){
        return instance.get(endpoint)
    },
    getAllByCategory: function(categoryId:number){
        return instance.get(endpoint+"/byCategory/"+categoryId)
    },
    getEventoById: function(id:number){
        return instance.get(endpoint+"/"+id)
    }
}