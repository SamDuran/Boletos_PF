export class Eventos {
    // #region PROPIEDADES
    boletos: Boletos[];
    boletosDisponibles: number;
    categoriaEventos: CategoriaEventos;
    categoriaId: number;
    creador: Usuarios;
    descripcion: string;
    foto: ArrayBuffer;
    eventoId: number;
    fechaEvento: Date = new Date();
    nombreEvento: string;
    secciones: Secciones[];
    ubicacion: Ubicaciones;
    ubicacionId: number;
    userId: number;

    // #endregion
}