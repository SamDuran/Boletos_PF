class Compras {
    // #region PROPIEDADES
    compraId: number;
    total: number;
    fechaCompra: Date;
    userEmail: string;
    detalles: dCompras[];
    // #endregion

    constructor() {
        this.compraId = this.total =  this.compraId = 0
        this.fechaCompra = new Date()
        this.userEmail = ''
        this.detalles = []
    }
}

class dCompras {
    // #region PROPIEDADES
    id: number;
    precio: number;
    cantidad: number;
    compraId: number;
    boletoId: number;
    // #endregion
    constructor() {
        this.id = this.precio = this.cantidad = this.compraId = this.boletoId = 0
    }
}