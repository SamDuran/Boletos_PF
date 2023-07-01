import { instance } from "./base.api";

const endpoint = 'api/Ubicaciones';

export const UbicacionesController = {
    getAll: function(){
        return instance.get(endpoint)
    },
    getLocationById: function(id:number){
        return instance.get(endpoint+"/"+id)
    }
}