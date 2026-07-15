import json
import math
import re
from datetime import datetime, date

import pandas as pd


EXCEL_PATH = r"C:\Users\reinhard2074\OneDrive - ARCADIS\Desktop\Tabellen automatisieren\WBS_AmprionPQ-bearbeitet.xlsx"
OUTPUT_PATH = r"C:\Projekte\wbs-tool\src\backend\WbsTool.Api\Data\Seed\amprion_pq_seed_data.json"


def is_blank(value):
    return pd.isna(value) or str(value).strip() == ""


def clean_str(value):
    if is_blank(value):
        return ""
    return str(value).strip()


def clean_id(value):
    if is_blank(value):
        return ""

    text = str(value).strip()

    if re.fullmatch(r"\d+\.0", text):
        return text[:-2]

    return text


def iso_date(value):
    if is_blank(value):
        return None

    if isinstance(value, (datetime, date)):
        if value.year < 2000:
            return None
        return value.strftime("%Y-%m-%d")

    text = str(value).strip()

    if text in {"0", "1"}:
        return None

    parsed = pd.to_datetime(text, errors="coerce")

    if pd.isna(parsed) or parsed.year < 2000:
        return None

    return parsed.strftime("%Y-%m-%d")


def num_or_none(value):
    if is_blank(value):
        return None

    try:
        numeric_value = float(value)

        if math.isnan(numeric_value):
            return None

        if abs(numeric_value - int(numeric_value)) < 0.000000001:
            return int(numeric_value)

        return round(numeric_value, 6)
    except Exception:
        return None


def map_node_type(value):
    mapping = {
        "Hauptpaket": "MainPackage",
        "Unterpaket": "SubPackage",
        "Aufgabe": "Task",
    }

    return mapping.get(clean_str(value), clean_str(value))


def map_status_code(status_label):
    mapping = {
        "": "Empty",
        "geliefert": "Delivered",
        "in Erstellung": "InCreation",
        "in Überarbeitung": "InReview",
        "kritisch": "Critical",
    }

    cleaned = clean_str(status_label)

    if cleaned in mapping:
        return mapping[cleaned]

    return cleaned.replace(" ", "") if cleaned else "Empty"


def assignment_role_from_row(row):
    if clean_str(row.get("Verantwortlich")).lower() == "ja":
        return "Responsible"

    if clean_str(row.get("Beratung_durch")).lower() == "ja":
        return "Consulted"

    if clean_str(row.get("Bearbeitung_durch")).lower() == "ja":
        return "Contributor"

    return "Contributor"


def main():
    wbs = pd.read_excel(
        EXCEL_PATH,
        sheet_name="WBS_Knoten",
        engine="openpyxl",
        dtype={"WBS_ID": str, "Parent_WBS_ID": str},
    )

    assignments_sheet = pd.read_excel(
        EXCEL_PATH,
        sheet_name="Ressourcen_Zuordnung",
        engine="openpyxl",
        dtype={"WBS_ID": str},
    )

    persons_sheet = pd.read_excel(
        EXCEL_PATH,
        sheet_name="Ressourcen",
        engine="openpyxl",
    )

    rates_sheet = pd.read_excel(
        EXCEL_PATH,
        sheet_name="Stundensätze",
        engine="openpyxl",
    )

    wbs_nodes = []

    for _, row in wbs.iterrows():
        visible_wbs_id = clean_id(row.get("WBS_ID"))

        if not visible_wbs_id:
            continue

        title = clean_str(row.get("Titel"))

        if not title:
            continue

        status_label = clean_str(row.get("Status"))

        wbs_nodes.append(
            {
                "visibleWbsId": visible_wbs_id,
                "parentVisibleWbsId": clean_id(row.get("Parent_WBS_ID")) or None,
                "level": num_or_none(row.get("WBS_Ebene")),
                "type": map_node_type(row.get("Typ")),
                "title": title,
                "statusLabel": status_label or "leer",
                "statusCode": map_status_code(status_label),
                "plannedStart": iso_date(row.get("Geplanter_Start")),
                "plannedEnd": iso_date(row.get("Geplantes_Ende")),
                "plannedDays": num_or_none(row.get("Geplante_Tage")),
                "plannedHoursLegacy": num_or_none(row.get("Geplante_Stunden")),
                "actualStart": iso_date(row.get("Ist_Start")),
                "actualEnd": iso_date(row.get("Ist_Ende")),
                "actualDays": num_or_none(row.get("Ist_Tage")),
                "actualHoursLegacy": num_or_none(row.get("Ist_Stunden")),
                "comment": clean_str(row.get("Kommentar")),
                "isBlocked": status_label.lower() == "kritisch",
                "isActive": True,
            }
        )

    rate_categories = []

    for _, row in rates_sheet.iterrows():
        code = clean_str(row.get("Kategorie"))

        if not code:
            continue

        rate_categories.append(
            {
                "code": code,
                "name": f"Kategorie {code}",
                "hourlyRate": num_or_none(row.get("Stundensatz (€)")),
                "currency": "EUR",
                "isActive": True,
            }
        )

    person_map = {}

    for _, row in persons_sheet.iterrows():
        name = clean_str(row.get("Name"))

        if not name:
            continue

        lower_name = name.lower()
        is_placeholder = lower_name in {"alle", "ingenieur"} or lower_name.startswith("nn")

        placeholder_type = None

        if lower_name == "alle":
            placeholder_type = "All"
        elif lower_name == "ingenieur":
            placeholder_type = "RolePlaceholder"

        person_map[name] = {
            "displayName": name,
            "shortName": name,
            "email": None,
            "defaultRole": clean_str(row.get("Rolle")),
            "defaultRateCategoryCode": clean_str(row.get("Stundensatz-Kat.")),
            "isPlaceholder": is_placeholder,
            "placeholderType": placeholder_type,
            "isActive": True,
        }

    for _, row in assignments_sheet.iterrows():
        name = clean_str(row.get("Person"))

        if not name or name in person_map:
            continue

        lower_name = name.lower()
        is_placeholder = lower_name in {"alle", "ingenieur"} or lower_name.startswith("nn")

        placeholder_type = None

        if lower_name == "alle":
            placeholder_type = "All"
        elif lower_name == "ingenieur":
            placeholder_type = "RolePlaceholder"

        person_map[name] = {
            "displayName": name,
            "shortName": name,
            "email": None,
            "defaultRole": clean_str(row.get("Rolle")),
            "defaultRateCategoryCode": clean_str(row.get("Stundensatz_Kategorie"))
            or clean_str(row.get("Ist_Stundensatz_Kategorie")),
            "isPlaceholder": is_placeholder,
            "placeholderType": placeholder_type,
            "isActive": True,
        }

    persons = list(person_map.values())
    persons.sort(key=lambda item: item["displayName"].lower())

    known_wbs_ids = {node["visibleWbsId"] for node in wbs_nodes}

    resource_assignments = []

    for _, row in assignments_sheet.iterrows():
        visible_wbs_id = clean_id(row.get("WBS_ID"))
        person_name = clean_str(row.get("Person"))

        if not visible_wbs_id or not person_name:
            continue

        if visible_wbs_id not in known_wbs_ids:
            continue

        resource_assignments.append(
            {
                "visibleWbsId": visible_wbs_id,
                "personDisplayName": person_name,
                "assignmentRole": assignment_role_from_row(row),
                "plannedRateCategoryCode": clean_str(row.get("Stundensatz_Kategorie")) or None,
                "actualRateCategoryCode": clean_str(row.get("Ist_Stundensatz_Kategorie")) or None,
                "plannedHours": num_or_none(row.get("Geplante_Stunden")),
                "actualHours": num_or_none(row.get("Ist_Stunden")),
                "comment": clean_str(row.get("Kommentar")),
                "isActive": True,
            }
        )

    task_statuses = [
        {
            "code": "Empty",
            "label": "leer",
            "color": "#94A3B8",
            "sortOrder": 10,
            "isActive": True,
            "isTerminal": False,
        },
        {
            "code": "InCreation",
            "label": "in Erstellung",
            "color": "#F59E0B",
            "sortOrder": 20,
            "isActive": True,
            "isTerminal": False,
        },
        {
            "code": "Delivered",
            "label": "geliefert",
            "color": "#3B82F6",
            "sortOrder": 30,
            "isActive": True,
            "isTerminal": False,
        },
        {
            "code": "InReview",
            "label": "in Überarbeitung",
            "color": "#8B5CF6",
            "sortOrder": 40,
            "isActive": True,
            "isTerminal": False,
        },
        {
            "code": "Critical",
            "label": "kritisch",
            "color": "#DC2626",
            "sortOrder": 50,
            "isActive": True,
            "isTerminal": False,
        },
        {
            "code": "Done",
            "label": "abgeschlossen",
            "color": "#16A34A",
            "sortOrder": 60,
            "isActive": True,
            "isTerminal": True,
        },
    ]

    seed_data = {
        "sourceFile": EXCEL_PATH,
        "project": {
            "projectNumber": "AMPRION-PQ-OHL",
            "name": "Amprion PQ Freileitung",
            "description": "Einmalige Seed-Migration aus WBS_AmprionPQ-bearbeitet.xlsx. Excel ist nicht führend; Datenbank ist Single Source of Truth.",
            "plannedStart": "2026-04-15",
            "plannedEnd": "2026-09-30",
        },
        "rateCategories": rate_categories,
        "taskStatuses": task_statuses,
        "persons": persons,
        "wbsNodes": wbs_nodes,
        "resourceAssignments": resource_assignments,
        "summary": {
            "wbsNodeCount": len(wbs_nodes),
            "resourceAssignmentCount": len(resource_assignments),
            "personCount": len(persons),
            "rateCategoryCount": len(rate_categories),
            "taskStatusCount": len(task_statuses),
        },
    }

    with open(OUTPUT_PATH, "w", encoding="utf-8") as file:
        json.dump(seed_data, file, ensure_ascii=False, indent=2)

    print("Seed-Datei erfolgreich erzeugt:")
    print(OUTPUT_PATH)
    print(json.dumps(seed_data["summary"], ensure_ascii=False, indent=2))


if __name__ == "__main__":
    main()