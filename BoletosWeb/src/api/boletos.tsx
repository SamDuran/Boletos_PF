import { instance } from "./base.api";
import "../../models/Eventos.tsx";

const endpoint = 'api/Boletos';

export const boletoController = {
    getBoletoBySeccion(seccionId: number) {
        return instance.get(endpoint + "/seccion/" + seccionId)
    }
}