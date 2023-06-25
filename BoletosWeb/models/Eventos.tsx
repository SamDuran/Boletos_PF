class Eventos{
// #region PROPIEDADES
    eventoId: number;
    boletosDisponibles : number;
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