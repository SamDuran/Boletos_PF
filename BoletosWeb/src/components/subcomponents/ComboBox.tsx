import React, { useState } from 'react';
import { Ubicaciones } from '../../../models/Ubicaciones';
import "./ComboBox.css"



interface ComboBoxProps {
    options: Ubicaciones[];
    onChange: (value: string) => void;
    placeholder: string;
    className : string;
}

const ComboBox: React.FC<ComboBoxProps> = ({ options, onChange, placeholder, className}) => {
    const [selectedValue, setSelectedValue] = useState<string>('');

    const handleValueChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const value = event.target.value;
        setSelectedValue(value);
        onChange(value);
    };

    return (
        <select value={selectedValue} placeholder={placeholder} className={className} onChange={handleValueChange}>
            <option value="Cualquier ubicacion">Cualquier ubicacion</option>
            {options.map((option) => (
                <option key={option.ubicacionId} value={option.ubicacion}>
                    {option.ubicacion}
                </option>
            ))}
        </select>
    );
};

export default ComboBox;
