import "./Home.style.css"
import Sidebar from "./subcomponents/sidebar";
import React, { useState } from 'react';
import { categorias } from '../api/categorias.tsx'
import { ComprasController } from '../api/compras.tsx'
import { BoletosComprados } from '../../models/Boletos.tsx'
import { Ubicaciones } from '../../models/Ubicaciones.tsx'
import { eventos } from '../api/eventos.tsx'
import { UbicacionesController } from "../api/ubicaciones.tsx";
import SearchBar from "./subcomponents/searchbar.tsx";
import Body from "./subcomponents/body.tsx"
import { Modal, ModalBody } from "reactstrap"
import { Eventos } from "../../models/Eventos.tsx"

export function Home() {
    /* Estados */
    const [categories, setCategories] = useState<CategoriaEventos[]>([]);
    const [events, setEventos] = useState<Eventos[]>([]);
    const [allEvents, setAllEventos] = useState<Eventos[]>([]);
    const [allLocations, setAllLocations] = useState<Ubicaciones[]>([]);
    const [categoryId, setCategory] = React.useState<number>(0)
    const [listaBoletos, setListaBoletos] = useState<BoletosComprados[]>([]);
    const [selectedDate, setSelectedDate] = useState<Date | null>(null);
    const [dialogVisible, setDialogVisible] = useState(false);

    /* Funciones */
    const handleBoletosAgregados = (boletos: BoletosComprados[]) => {
        setListaBoletos((prevLista) => [...prevLista, ...boletos]);
    };
    const handleClick = (numero: number) => {
        setCategory(numero);
    };
    const onSearchButtonClick = (input: string) => {

        setEventos(allEvents.filter((evento) => {
            if (selectedDate == null)
                if (input.length > 0)
                    return (evento.nombreEvento.includes(input) || evento.descripcion.includes(input))
                else
                    return evento
            else
                console.log('fecha seleccionada: (' + selectedDate + ") y fecha del evento: " + evento.fechaEvento + ")")

            if (input.length > 0)
                return (evento.nombreEvento.includes(input) || evento.descripcion.includes(input)) && evento.fechaEvento >= selectedDate
            else

                return evento.fechaEvento === selectedDate
        }));
    };
    const onCompraCheck = () => {
        setDialogVisible(true);
    };
    const closeDialog = () => {
        setDialogVisible(false);
    };
    const onUbicacionChange = (ubicacion: string) => {
        if (ubicacion == 'Cualquier ubicacion') {
            setEventos(allEvents);
        }
        else {
            setEventos(allEvents.filter((evento) => {
                return evento.ubicacion.ubicacion == ubicacion
            }));
        }

    };
    const EliminarDetalle = (boletoId: number) => {
        const nuevaListaCompra = listaBoletos.filter(
            (compra) => compra.boleto.boletoId !== boletoId
        );
        setListaBoletos(nuevaListaCompra);
    };
    React.useEffect(() => {
        categorias
            .getAll().then((r) => {
                setCategories(r.data);
            })
            .catch((e) => {
                console.error(e);
            });

        eventos
            .getAllByCategory(categoryId).then((r) => {
                setEventos(r.data);
            })
            .catch((e) => {
                console.error(e);
            });

        eventos
            .getAll().then((r) => {
                setAllEventos(r.data);
            })
            .catch((e) => {
                console.error(e);
            });
        UbicacionesController
            .getAll().then((r) => {
                setAllLocations(r.data);
            }).catch((e) => {
                console.error(e)
            })

    }, [categoryId]);

    return (
        <>
            <article className="article-header">
                <img className="profile-icon" src="/src/components/resources/profileIcon.png"></img>
                <header className="header">
                    <img src="/src/components/resources/BoletosPF_Icon.png" className="header-logo" />
                    <h1 className="header-title">Ticket tracker</h1>
                    <h5 className="header-subtitle">¡Compra tus boletos sin límites, sin estrés!</h5>
                </header>
            </article>
            <section>
                <div className="page-body">
                    <Sidebar lista={categories} onCategoryClick={handleClick} />
                    <div className="body-col">
                        <SearchBar
                            OnSearch={onSearchButtonClick}
                            OnCheckCompraClick={onCompraCheck}
                            OnLocationChange={onUbicacionChange}
                            locations={allLocations}
                            selectedDate={selectedDate}
                            setSelectedDate={setSelectedDate}
                        />
                        <Body lista={events} OnBoletosAgregados={handleBoletosAgregados} />
                    </div>
                </div>
            </section>
            {dialogVisible && <CarritoDialog onClose={closeDialog} opened={dialogVisible} listaCompra={listaBoletos} onEliminarDetalle={EliminarDetalle} />}
        </>
    );
}


function CarritoDialog({ onClose, opened, listaCompra, onEliminarDetalle }: { onClose: () => void, opened: boolean, listaCompra: BoletosComprados[], onEliminarDetalle: (boletoId: number) => void }) {

    const [email, setEmail] = React.useState<string>("")
    const emailRegex = /^(?=.{1,254}$)(?=.{1,64}@)[-!#$%&'*+/0-9=?A-Z^_`a-z{|}~]+(\.[-!#$%&'*+/0-9=?A-Z^_`a-z{|}~]+)*@[A-Za-z0-9]([A-Za-z0-9-]{0,61}[A-Za-z0-9])?(\.[A-Za-z0-9]([A-Za-z0-9-]{0,61}[A-Za-z0-9])?)*$/;
    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setEmail(event.target.value);
    };
    const onCerrarClick = () => {
        onClose()
    };
    const comprar = async () => {
        if (!email.match(emailRegex)) {
            alert("Debes colocar un email valido")
            return;
        }
        try {
            const res = await ComprasController.buyBoletos(email, listaCompra);
            if (res.data == false) {
                alert("No paso")
            }
        } catch (error) {
            console.log(error)
        }
    };

    return (
        <>
            <Modal isOpen={opened} className="scrim">
                <div className="closeRow">
                    <label onClick={onCerrarClick} className="cerrarBTN">X</label>
                </div>
                <ModalBody className="dialog" >
                    <h2 className="title">Carrito de compras</h2>
                    {/* Correo div */}
                    <label>Correo: </label>
                    <div className="correo-field" >
                        <input className="correo-input" value={email} onChange={handleInputChange} type="text"></input>
                    </div>

                    {/* Lista de detalles div */}
                    <div className="detalles-div" >
                        {listaCompra.map((compra) => (
                            <DetalleRow key={compra.boleto.boletoId} detalle={compra} onEliminarDetalle={onEliminarDetalle} />
                        ))}
                    </div>
                    {/* botones div */}
                    <div className="buttons-panel" >
                        <button onClick={onClose} className="bttn">Cancelar</button>
                        <button onClick={comprar} className="bttn" >Comprar</button>
                    </div>
                </ModalBody>
            </Modal>
        </>
    );
}
export default Home
function DetalleRow({ detalle, onEliminarDetalle }: { detalle: BoletosComprados, onEliminarDetalle: ((id: number) => void) }) {

    return (
        <>
            <div className="detalle-bg">
                <div className="detalle-row">
                    {/* Presentacion DIV */}
                    <div className="detalle-card" >
                        <span>Evento: {detalle.boleto.eventoId}</span>
                        <span>Seccion: {detalle.boleto.seccionId}</span>
                        <span>Precio: {detalle.precio}</span>
                    </div>
                    {/* Modificacion DIV */}
                    <div className="row">
                        {/* Cantidad DIV */}
                        <div className="cantidad-span">
                            <span  >Cantidad: {detalle.cantidadComprada}</span>
                        </div>
                        <button className="eliminarBTN" onClick={() => onEliminarDetalle(detalle.boleto.boletoId)} >Eliminar</button>
                    </div>
                </div>
            </div>
        </>
    );
}
