
import {instance} from "./base.api"
import {BoletosComprados} from "../../models/Boletos"

export const ComprasController = {
    buyBoletos(email:string, boletos : BoletosComprados[]){
        return instance.post("/BuyBoletos", new compraDTO(email,boletos))
    }
}

class compraDTO{
    userEmail = '';
    details : detalleCompra[] = []
    constructor(email:string, boletos: BoletosComprados[]){
        this.userEmail = email;
        boletos.forEach((boleto) => {
            this.details.push(new detalleCompra(boleto.precio,boleto.cantidadComprada,boleto.boleto.boletoId));
        })
    }
}

class detalleCompra{
    precio = 0;
    cantidad = 0;
    boletoId = 0;

    constructor(precio : number, cantidad: number, boletoId : number ){
        this.precio = precio;
        this.cantidad = cantidad;
        this.boletoId = boletoId
    }
}