class Compras {
// #region PROPIEDADES
    compraId : number; 
    total : number;
    fechaCompra : Date;
    userEmail : string;
    detalles : dCompras[];
// #endregion
}

class dCompras {
// #region PROPIEDADES
    id : number;
    precio : number;
    cantidad : number;
    compraId : number;
    boletoId : number;
// #endregion
}