import { Modal, ModalBody } from "reactstrap"
import "./compraDialog.css"
import { Secciones, SeccionCantidad } from "../../models/Secciones"
import "../../models/Compras.tsx"
import { boletoController } from '../api/boletos.tsx'
import { Boletos, BoletosComprados } from '../../models/Boletos.tsx'

import { useState, useEffect } from "react"

function CompraDialog({ evento, onClose, opened, OnBoletosAgregados }: { evento: Eventos, onClose: () => void, opened: boolean, OnBoletosAgregados: (boletos: BoletosComprados[]) => void }) {


    const [boletos, addBoleto] = useState<BoletosComprados[]>([]);
    let sumaCompra = 0;
    const [seccionesCantidad, setSeccionesCantidad] = useState<SeccionCantidad[]>([]);

    const actualizarCantidad = (seccionId: number, cantidad: number, precio: number) => {
        const seccionExistente = seccionesCantidad.find(sc => sc.seccionId === seccionId);

        if (seccionExistente) {

            const nuevasSecciones = seccionesCantidad.map(sc => {
                if (sc.seccionId === seccionId) {
                    return new SeccionCantidad(sc.seccionId, cantidad, precio);
                }
                return sc;
            });

            setSeccionesCantidad(nuevasSecciones);
        } else {

            const nuevasSecciones = [...seccionesCantidad, new SeccionCantidad(seccionId, cantidad, precio)];
            setSeccionesCantidad(nuevasSecciones);
        }
    };

    useEffect(() => {
        const handleKeyPress = (event: KeyboardEvent) => {
            if (event.key == 'Escape') {
                onClose()
            }
        };
        window.addEventListener('keydown', handleKeyPress);
        return () => {
            window.removeEventListener('keydown', handleKeyPress);
        };
    }, [onClose]);

    const onCerrarClick = () => {
        onClose()
    };

    const agregarBoleto = (boleto: Boletos, cantidadagregada: number, precio: number, lista: BoletosComprados[]) => {
        lista.push(new BoletosComprados(boleto, cantidadagregada, precio))
    }
    const imageUrl = URL.createObjectURL(new Blob([evento.foto]));
    const OnAgregarClick = async () => {
        for (const seccionCantidad of seccionesCantidad) {
            sumaCompra += seccionCantidad.cantidad
        }
        if (sumaCompra > evento.boletosDisponibles) {
            alert('La cantidad deseada sobrepasa la cantidad disponible de boletos.')
            return
        }
        try {
            const nuevosBoletos: BoletosComprados[] = [];
            for (const seccionCantidad of seccionesCantidad) {
                const res = await boletoController.getBoletoBySeccion(seccionCantidad.seccionId);
                const boletoData = res.data;

                agregarBoleto(boletoData, seccionCantidad.cantidad, seccionCantidad.precio, nuevosBoletos);
            }
            OnBoletosAgregados(nuevosBoletos);
            addBoleto((prevBoletos) => [...prevBoletos, ...nuevosBoletos]);
            onClose();
        } catch (error) {
            console.log(error);
        }

    };
    const unsafeImg = URL.createObjectURL(new Blob([evento.foto]));
    const fecha = new Date(evento.fechaEvento).toISOString().substring(0, 10);
    const lista = evento.secciones;
    return (
        <>
            <Modal isOpen={opened} className="scrimm">
                <div className="cerrarRow">
                    <div onClick={onCerrarClick} className="CerrarBTN">X</div>
                </div>
                <ModalBody className="dialogBody" >
                    {/* PRESENTACION DIV */}
                    <div className="presentation-div">
                        {evento.foto != null && <img src={unsafeImg} className='event-img'></img>}
                        
                        {evento.foto == null && <img src='/src/components/resources/photo.png' className='event-img'></img>}
                        {/* Datos DIV */}
                        <div className="data-div">
                            <div className="data-header">
                                <span className="name">{evento.nombreEvento}</span>
                                <span >{'Fecha del evento: '} {fecha}</span>
                            </div>
                            <span className="description">{evento.descripcion}</span>
                        </div>
                        <span>Boletos disponibles:
                            {evento.boletosDisponibles == 0 && <span className="red"> {evento.boletosDisponibles}</span>}
                            {evento.boletosDisponibles > 0 && <span > {evento.boletosDisponibles}</span>}
                        </span>

                    </div>
                    {/* SECCIONES DIV */}
                    <span>SECCIONES</span>
                    <div className="separator" />
                    <div className="sections-div">
                        <div className="seccion-area">
                            {lista.map((seccion) => (
                                <SeccionCard
                                    key={seccion.seccionId}
                                    seccion={seccion}
                                    cantidad={seccionesCantidad[seccion.seccionId]?.cantidad || 0}
                                    actualizarCantidad={actualizarCantidad}
                                />
                            ))}
                        </div>

                    </div>
                    {/* BOTONES */}
                    <div className="button-panel">
                        <button onClick={onCerrarClick} className="btn" >Cancelar</button>
                        {evento.boletosDisponibles > 0 && <button className="btn" onClick={OnAgregarClick} >Agregar a carrito</button>}
                    </div>
                </ModalBody>
            </Modal>

        </>
    );
}

export default CompraDialog

function SeccionCard({ seccion, cantidad, actualizarCantidad }: { seccion: Secciones; cantidad: number; actualizarCantidad: (seccionId: number, cantidad: number, precio: number) => void }) {
    const [newCantidad, setCantidad] = useState(cantidad);

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const inputCantidad = event.target.valueAsNumber;

        if (inputCantidad >= 0) {
            setCantidad(inputCantidad);
            actualizarCantidad(seccion.seccionId, inputCantidad, seccion.precio);
        }
    };

    const lessCantidad = () => {
        if (newCantidad > 0) {
            const updatedCantidad = newCantidad - 1;
            setCantidad(updatedCantidad);
            actualizarCantidad(seccion.seccionId, updatedCantidad, seccion.precio);
        }
    };

    const addCantidad = () => {
        const updatedCantidad = newCantidad + 1;
        setCantidad(updatedCantidad);
        actualizarCantidad(seccion.seccionId, updatedCantidad, seccion.precio);
    };
    return (
        <>
            <div className="seccion-card">
                <div className="seccion-title">
                    <span  > {seccion.seccion} </span>
                </div>
                <div className="label precio" >
                    Precio: {seccion.precio}
                </div>
                <div className="inputs">
                    <span className="label">Cantidad</span>
                    <div className="fieldc-bg" >
                        <input className="fieldc" value={newCantidad} onChange={handleInputChange} type="number" />
                    </div>
                    <div className="buttons">
                        <button onClick={lessCantidad} className="button button-menos">-</button>
                        <button onClick={addCantidad} className="button button-mas">+</button>
                    </div>
                </div>
            </div>
        </>
    );
}
