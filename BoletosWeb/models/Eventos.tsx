class Eventos{
// #region PROPIEDADES
    eventoId: number;
    boletosDisponibles : number;
    fechaEvento: Date = new Date();
    nombreEvento : string;
    descripcion : string;
    userId : number;
    creador : Eventos;
    categoriaId : number; 
    categoria : CategoriaEventos;
    ubicacionId : number;
    ubicacion : Ubicaciones; 
    secciones : Secciones[];
    boletos : Boletos[];

// #endregion
}