export class Boletos {
    // #region PROPIEDADES
    boletoId = 0;
    eventoId = 0;
    seccionId = 0;
    // #endregion
    constructor(boletoId: number, eventoId: number, seccionId: number) {
        this.boletoId = boletoId;
        this.eventoId = eventoId;
        this.seccionId = seccionId;
    }
}

export class BoletosComprados {
    boleto: Boletos = new Boletos(0,0,0);
    cantidadComprada = 0;
    precio = 0;
    constructor(boleto: Boletos, cantidad: number, precio : number){
        this.boleto = boleto;
        this.cantidadComprada = cantidad
        this.precio = precio
    }
}