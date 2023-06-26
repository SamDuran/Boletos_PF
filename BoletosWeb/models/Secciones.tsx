export class Secciones { 
// #region PROPIEDADES
    seccionId  = 0;
    eventoId= 0;
    seccion = "";
    precio= 0;
// #endregion
}

export class SeccionCantidad{
    seccionId: number;
    cantidad: number;
    precio: number;

    constructor(seccion: number, cantidad : number, precio:number){
        this.seccionId = seccion;
        this.precio = precio;
        this.cantidad = cantidad;
    }
}